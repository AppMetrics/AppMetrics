using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Internal
{
    internal sealed class DefaultMetricsRegistry : IMetricsRegistry
    {
        private readonly IClock _clock;
        private readonly ILogger _logger;
        private readonly string _defaultGroupName;
        private readonly SamplingType _defaultSamplingType;
        private readonly EnvironmentInfoBuilder _environmentInfoBuilder;

        private readonly ConcurrentDictionary<string, IMetricGroupRegistry> _groups = new ConcurrentDictionary<string, IMetricGroupRegistry>();
        private readonly Func<string, IMetricGroupRegistry> _newGroupRegistry;

        public DefaultMetricsRegistry(
            ILoggerFactory loggerFactory,
            IOptions<AppMetricsOptions> options,
            EnvironmentInfoBuilder environmentInfoBuilder,
            Func<string, IMetricGroupRegistry> newGroupRegistry)
        {
            _logger = loggerFactory.CreateLogger<DefaultMetricsRegistry>();
            _environmentInfoBuilder = environmentInfoBuilder;
            _newGroupRegistry = newGroupRegistry;
            _defaultGroupName = options.Value.DefaultGroupName;
            _defaultSamplingType = options.Value.DefaultSamplingType;
            _clock = options.Value.Clock;
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

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterImplementation
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

        public async Task<MetricsData> GetDataAsync()
        {
            _logger.MetricsDataGetExecuting();

            var environment = await _environmentInfoBuilder.BuildAsync();

            //TODO: AH - clean this up

            if (_groups.Count == 0)
            {
                return MetricsData.Empty;
            }

            //TODO: AH- Remove concept of ChildContext (groups) dont see much need in having parent child relationship
            var parentRegistry = _groups.Values.FirstOrDefault(g => g.GroupName == _defaultGroupName);

            if (parentRegistry == null)
            {
                parentRegistry = _newGroupRegistry(_defaultGroupName);
                parentRegistry = _groups.GetOrAdd(_defaultGroupName, parentRegistry);
            }

            MetricsData data;

            if (_groups.Count > 1)
            {
                var childRegistries = _groups.Values.Where(g => g.GroupName != _defaultGroupName).Select(g =>
                    new MetricsData(g.GroupName, _clock.UtcDateTime, environment.Entries,
                        g.DataProvider.Gauges.ToArray(),
                        g.DataProvider.Counters.ToArray(),
                        g.DataProvider.Meters.ToArray(),
                        g.DataProvider.Histograms.ToArray(),
                        g.DataProvider.Timers.ToArray(), Enumerable.Empty<MetricsData>()));

                data = new MetricsData(parentRegistry.GroupName, _clock.UtcDateTime, environment.Entries,
                    parentRegistry.DataProvider.Gauges.ToArray(),
                    parentRegistry.DataProvider.Counters.ToArray(),
                    parentRegistry.DataProvider.Meters.ToArray(),
                    parentRegistry.DataProvider.Histograms.ToArray(),
                    parentRegistry.DataProvider.Timers.ToArray(),
                    childRegistries);
            }
            else
            {
                data = new MetricsData(parentRegistry.GroupName, _clock.UtcDateTime, environment.Entries,
                    parentRegistry.DataProvider.Gauges.ToArray(),
                    parentRegistry.DataProvider.Counters.ToArray(),
                    parentRegistry.DataProvider.Meters.ToArray(),
                    parentRegistry.DataProvider.Histograms.ToArray(),
                    parentRegistry.DataProvider.Timers.ToArray(),
                    Enumerable.Empty<MetricsData>());
            }

            _logger.MetricsDataGetExecuted();

            return data;
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation
        {
            EnsureGroupName(options);
            EnsureSamplingType(options);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            return registry.Histogram(options, builder);
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation
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

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerImplementation
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