// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Registries;
using App.Metrics.Utils;

namespace App.Metrics.Internal
{
    internal sealed class DefaultMetricsRegistry : IMetricsRegistry
    {
        private readonly IClock _clock;
        private readonly string _defaultGroupName;
        private readonly SamplingType _defaultSamplingType;
        private readonly EnvironmentInfo _environment;
        private readonly ConcurrentDictionary<string, IMetricGroupRegistry> _groups = new ConcurrentDictionary<string, IMetricGroupRegistry>();
        private readonly Func<string, IMetricGroupRegistry> _newGroupRegistry;

        public DefaultMetricsRegistry(string defaultGroupName,
            SamplingType defaultSamplingType,
            IClock clock,
            EnvironmentInfo environment,
            Func<string, IMetricGroupRegistry> newGroupRegistry)
        {
            _defaultGroupName = defaultGroupName;
            _defaultSamplingType = defaultSamplingType;
            _clock = clock;
            _environment = environment;
            _newGroupRegistry = newGroupRegistry;

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
            options.EnsureGroupName(_defaultGroupName);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            return registry.Counter(options, builder);
        }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
        {
            options.EnsureGroupName(_defaultGroupName);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            registry.Gauge(options, valueProvider);
        }

        public MetricsData GetData()
        {
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
                    new MetricsData(g.GroupName, _clock.UtcDateTime, _environment.Entries,
                        g.DataProvider.Gauges.ToArray(),
                        g.DataProvider.Counters.ToArray(),
                        g.DataProvider.Meters.ToArray(),
                        g.DataProvider.Histograms.ToArray(),
                        g.DataProvider.Timers.ToArray(), Enumerable.Empty<MetricsData>()));

                data = new MetricsData(parentRegistry.GroupName, _clock.UtcDateTime, _environment.Entries,
                    parentRegistry.DataProvider.Gauges.ToArray(),
                    parentRegistry.DataProvider.Counters.ToArray(),
                    parentRegistry.DataProvider.Meters.ToArray(),
                    parentRegistry.DataProvider.Histograms.ToArray(),
                    parentRegistry.DataProvider.Timers.ToArray(),
                    childRegistries);
            }
            else
            {
                data = new MetricsData(parentRegistry.GroupName, _clock.UtcDateTime, _environment.Entries,
                    parentRegistry.DataProvider.Gauges.ToArray(),
                    parentRegistry.DataProvider.Counters.ToArray(),
                    parentRegistry.DataProvider.Meters.ToArray(),
                    parentRegistry.DataProvider.Histograms.ToArray(),
                    parentRegistry.DataProvider.Timers.ToArray(),
                    Enumerable.Empty<MetricsData>());
            }


            return data;
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation
        {
            options.EnsureGroupName(_defaultGroupName);
            options.EnsureSamplingType(_defaultSamplingType);
            var registry = _groups.GetOrAdd(options.GroupName, _newGroupRegistry);
            return registry.Histogram(options, builder);
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation
        {
            options.EnsureGroupName(_defaultGroupName);
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
            options.EnsureGroupName(_defaultGroupName);
            options.EnsureSamplingType(_defaultSamplingType);
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

    internal sealed class DefaultMetricGroupRegistry : IMetricGroupRegistry
    {
        private readonly MetricMetaCatalog<ICounter, CounterValueSource, CounterValue> _counters =
            new MetricMetaCatalog<ICounter, CounterValueSource, CounterValue>();

        private readonly MetricMetaCatalog<IMetricValueProvider<double>, GaugeValueSource, double> _gauges =
            new MetricMetaCatalog<IMetricValueProvider<double>, GaugeValueSource, double>();

        private readonly MetricMetaCatalog<IHistogram, HistogramValueSource, HistogramValue> _histograms =
            new MetricMetaCatalog<IHistogram, HistogramValueSource, HistogramValue>();

        private readonly MetricMetaCatalog<IMeter, MeterValueSource, MeterValue> _meters =
            new MetricMetaCatalog<IMeter, MeterValueSource, MeterValue>();

        private readonly MetricMetaCatalog<ITimer, TimerValueSource, TimerValue> _timers =
            new MetricMetaCatalog<ITimer, TimerValueSource, TimerValue>();

        public DefaultMetricGroupRegistry(string groupName)
        {
            if (groupName.IsMissing())
            {
                throw new ArgumentException("Registry GroupName cannot be null or empty", nameof(groupName));
            }

            GroupName = groupName;

            DataProvider = new DefaultMetricRegistryManager(
                () => _gauges.All,
                () => _counters.All,
                () => _meters.All,
                () => _histograms.All,
                () => _timers.All);
        }

        public IMetricRegistryManager DataProvider { get; }

        public string GroupName { get; }

        public void ClearAllMetrics()
        {
            _gauges.Clear();
            _counters.Clear();
            _meters.Clear();
            _histograms.Clear();
            _timers.Clear();
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterImplementation
        {
            return _counters.GetOrAdd(options.Name, () =>
            {
                var counter = builder();

                return Tuple.Create((ICounter)counter,
                    new CounterValueSource(options.Name, counter, options.MeasurementUnit, options.Tags));
            });
        }

        public void Gauge(GaugeOptions gaugeOptions, Func<IMetricValueProvider<double>> valueProvider)
        {
            _gauges.GetOrAdd(gaugeOptions.Name, () =>
            {
                var gauge = valueProvider();
                return Tuple.Create(gauge, new GaugeValueSource(gaugeOptions.Name, gauge, gaugeOptions.MeasurementUnit, gaugeOptions.Tags));
            });
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation
        {
            return _histograms.GetOrAdd(options.Name, () =>
            {
                var histogram = builder();
                return Tuple.Create((IHistogram)histogram, new HistogramValueSource(options.Name, histogram, options.MeasurementUnit, options.Tags));
            });
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation
        {
            return _meters.GetOrAdd(options.Name, () =>
            {
                var meter = builder();
                return Tuple.Create((IMeter)meter, new MeterValueSource(options.Name, meter, options.MeasurementUnit, options.RateUnit, options.Tags));
            });
        }

        public void ResetMetricsValues()
        {
            _gauges.Reset();
            _counters.Reset();
            _meters.Reset();
            _histograms.Reset();
            _timers.Reset();
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerImplementation
        {
            return _timers.GetOrAdd(options.Name, () =>
            {
                var timer = builder();
                return Tuple.Create((ITimer)timer,
                    new TimerValueSource(options.Name, timer, options.MeasurementUnit, options.RateUnit, options.DurationUnit, options.Tags));
            });
        }

        private class MetricMetaCatalog<TMetric, TValue, TMetricValue>
            where TValue : MetricValueSource<TMetricValue>
        {
            private readonly ConcurrentDictionary<string, MetricMeta> metrics =
                new ConcurrentDictionary<string, MetricMeta>();

            public IEnumerable<TValue> All
            {
                get { return metrics.Values.OrderBy(m => m.Name).Select(v => v.Value); }
            }

            public void Clear()
            {
                var values = metrics.Values;
                metrics.Clear();
                foreach (var value in values)
                {
                    using (value.Metric as IDisposable)
                    {
                    }
                }
            }

            public TMetric GetOrAdd(string name, Func<Tuple<TMetric, TValue>> metricProvider)
            {
                return metrics.GetOrAdd(name, n =>
                {
                    var result = metricProvider();
                    return new MetricMeta(result.Item1, result.Item2);
                }).Metric;
            }

            public void Reset()
            {
                foreach (var metric in metrics.Values)
                {
                    var resetable = metric.Metric as IResetableMetric;
                    resetable?.Reset();
                }
            }

            private class MetricMeta
            {
                public MetricMeta(TMetric metric, TValue valueUnit)
                {
                    Metric = metric;
                    Value = valueUnit;
                }

                public TMetric Metric { get; }

                public string Name => Value.Name;

                public TValue Value { get; }
            }
        }
    }
}