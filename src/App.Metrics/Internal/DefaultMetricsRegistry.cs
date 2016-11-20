// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Data;
using App.Metrics.Data.Interfaces;
using App.Metrics.Infrastructure;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal
{
    internal sealed class DefaultMetricsRegistry : IMetricsRegistry
    {
        private readonly IClock _clock;

        private readonly ConcurrentDictionary<string, IMetricContextRegistry> _contexts = new ConcurrentDictionary<string, IMetricContextRegistry>();
        private readonly string _defaultContextLabel;
        private readonly SamplingType _defaultSamplingType;
        private readonly EnvironmentInfoProvider _environmentInfoProvider;
        private readonly ILogger _logger;
        private readonly Func<string, IMetricContextRegistry> _newContextRegistry;
        private readonly MetricTags _globalTags;

        public DefaultMetricsRegistry(
            ILoggerFactory loggerFactory,
            AppMetricsOptions options,
            IClock clock,
            EnvironmentInfoProvider environmentInfoProvider,
            Func<string, IMetricContextRegistry> newContextRegistry)
        {
            _logger = loggerFactory.CreateLogger<DefaultMetricContextRegistry>();
            _environmentInfoProvider = environmentInfoProvider;
            _clock = clock;
            _newContextRegistry = newContextRegistry;
            _defaultContextLabel = options.DefaultContextLabel;
            _defaultSamplingType = options.DefaultSamplingType;
            _contexts.TryAdd(_defaultContextLabel, newContextRegistry(_defaultContextLabel));
            _globalTags = new MetricTags(options.GlobalTags);
        }

        public bool AddContext(string context, IMetricContextRegistry registry)
        {
            if (context.IsMissing())
            {
                throw new ArgumentException("Registry Context cannot be null or empty", nameof(context));
            }

            var attached = _contexts.GetOrAdd(context, registry);

            return ReferenceEquals(attached, registry);
        }

        public void Clear()
        {
            ForAllContexts(c =>
            {
                c.ClearAllMetrics();
                _contexts.TryRemove(c.Context, out c);
            });
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric
        {
            EnsureContextLabelAndGlobalTags(options);
            var registry = _contexts.GetOrAdd(options.Context, _newContextRegistry);
            return registry.Counter(options, builder);
        }

        public MetricValueOptions EnsureContextLabelAndGlobalTags(MetricValueOptions options)
        {
            if (options.Context.IsMissing())
            {
                options.Context = _defaultContextLabel;
            }

            //TODO: AH - impure method called
            options.Tags.With(_globalTags.ToDictionary());

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
            EnsureContextLabelAndGlobalTags(options);
            var registry = _contexts.GetOrAdd(options.Context, _newContextRegistry);
            registry.Gauge(options, valueProvider);
        }

        public async Task<MetricsDataValueSource> GetDataAsync(IMetricsFilter filter)
        {
            _logger.RetrievedMetricsData();

            if (_contexts.Count == 0)
            {
                return MetricsDataValueSource.Empty;
            }

            var environment = await _environmentInfoProvider.BuildAsync();

            var contexts = _contexts.Values.Select(g => new MetricsContextValueSource(
                g.Context,
                g.DataProvider.Gauges.ToArray(),
                g.DataProvider.Counters.ToArray(),
                g.DataProvider.Meters.ToArray(),
                g.DataProvider.Histograms.ToArray(),
                g.DataProvider.Timers.ToArray()
            ));

            var data = new MetricsDataValueSource(_clock.UtcDateTime, environment, contexts);

            _logger.GettingMetricsData();

            return data.Filter(filter);
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric
        {
            EnsureContextLabelAndGlobalTags(options);
            EnsureSamplingType(options);
            var registry = _contexts.GetOrAdd(options.Context, _newContextRegistry);
            return registry.Histogram(options, builder);
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric
        {
            EnsureContextLabelAndGlobalTags(options);
            var registry = _contexts.GetOrAdd(options.Context, _newContextRegistry);
            return registry.Meter(options, builder);
        }

        public void RemoveContext(string context)
        {
            if (context.IsMissing())
            {
                throw new ArgumentException("Registry Context cannot be null or empty", nameof(context));
            }

            IMetricContextRegistry registry;
            if (_contexts.TryRemove(context, out registry))
            {
                registry.ClearAllMetrics();
            }
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric
        {
            EnsureContextLabelAndGlobalTags(options);
            EnsureSamplingType(options);
            var registry = _contexts.GetOrAdd(options.Context, _newContextRegistry);
            return registry.Timer(options, builder);
        }

        private void ForAllContexts(Action<IMetricContextRegistry> action)
        {
            foreach (var context in _contexts.Values)
            {
                action(context);
            }
        }
    }
}