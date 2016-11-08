// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Infrastructure;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal
{
    internal sealed class DefaultMetricsRegistry : IMetricsRegistry
    {
        private readonly IClock _clock;
        private readonly string _defaultGroupName;
        private readonly SamplingType _defaultSamplingType;
        private readonly EnvironmentInfoBuilder _environmentInfoBuilder;
        private readonly string _globalContextName;

        private readonly ConcurrentDictionary<string, IMetricGroupRegistry> _groups = new ConcurrentDictionary<string, IMetricGroupRegistry>();
        private readonly ILogger _logger;
        private readonly Func<string, IMetricGroupRegistry> _newGroupRegistry;

        public DefaultMetricsRegistry(
            ILoggerFactory loggerFactory,
            AppMetricsOptions options,
            EnvironmentInfoBuilder environmentInfoBuilder,
            Func<string, IMetricGroupRegistry> newGroupRegistry)
        {
            _logger = loggerFactory.CreateLogger<DefaultMetricGroupRegistry>();
            _environmentInfoBuilder = environmentInfoBuilder;
            _newGroupRegistry = newGroupRegistry;
            _defaultGroupName = options.DefaultGroupName;
            _defaultSamplingType = options.DefaultSamplingType;
            _globalContextName = options.GlobalContextName;
            _clock = options.Clock;
            _groups.TryAdd(_defaultGroupName, newGroupRegistry(_defaultGroupName));
        }

        public bool AddGroup(string groupName, IMetricGroupRegistry registry)
        {
            if (groupName.IsMissing())
            {
                throw new ArgumentException("Registry GroupName cannot be null or empty", nameof(groupName));
            }

            var attached = _groups.GetOrAdd(groupName, registry);

            return ReferenceEquals(attached, registry);
        }

        public void Clear()
        {
            ForAllGroups(c =>
            {
                c.ClearAllMetrics();
                _groups.TryRemove(c.GroupName, out c);
            });
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric
        {
            EnsureGroupName(options);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            return registry.Counter(options, builder);
        }

        public MetricValueOptions EnsureGroupName(MetricValueOptions options)
        {
            if (options.GroupName.IsMissing())
            {
                options.GroupName = _defaultGroupName;
            }

            return options;
        }

        public MetricValueWithSamplingOption EnsureSamplingType(MetricValueWithSamplingOption options)
        {
            if (options.SamplingType == SamplingType.Default)
            {
                options.SamplingType = _defaultSamplingType;
            }

            return options;
        }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
        {
            EnsureGroupName(options);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            registry.Gauge(options, valueProvider);
        }

        public async Task<MetricsDataValueSource> GetDataAsync()
        {
            _logger.MetricsDataGetExecuting();

            var environment = await _environmentInfoBuilder.BuildAsync();

            if (_groups.Count == 0)
            {
                return MetricsDataValueSource.Empty;
            }

            var metricDataGroups = _groups.Values.Select(g => new MetricsDataGroupValueSource(
                g.GroupName,
                g.DataProvider.Gauges.ToArray(),
                g.DataProvider.Counters.ToArray(),
                g.DataProvider.Meters.ToArray(),
                g.DataProvider.Histograms.ToArray(),
                g.DataProvider.Timers.ToArray()
            ));

            var data = new MetricsDataValueSource(_globalContextName, _clock.UtcDateTime, environment, metricDataGroups);

            _logger.MetricsDataGetExecuted();

            return data;
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric
        {
            EnsureGroupName(options);
            EnsureSamplingType(options);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            return registry.Histogram(options, builder);
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric
        {
            EnsureGroupName(options);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            return registry.Meter(options, builder);
        }

        public void RemoveGroup(string groupName)
        {
            if (groupName.IsMissing())
            {
                throw new ArgumentException("Registry GroupName cannot be null or empty", nameof(groupName));
            }

            IMetricGroupRegistry registry;
            if (_groups.TryRemove(groupName, out registry))
            {
                //TODO: AH - should this dispose the registry?
                registry.ClearAllMetrics();
            }
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric
        {
            EnsureGroupName(options);
            EnsureSamplingType(options);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            return registry.Timer(options, builder);
        }

        private void ForAllGroups(Action<IMetricGroupRegistry> action)
        {
            foreach (var group in _groups.Values)
            {
                action(group);
            }
        }
    }
}