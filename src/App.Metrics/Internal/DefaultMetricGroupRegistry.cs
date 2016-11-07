// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Data;

namespace App.Metrics.Internal
{
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

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric
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

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric
        {
            return _histograms.GetOrAdd(options.Name, () =>
            {
                var histogram = builder();
                return Tuple.Create((IHistogram)histogram, new HistogramValueSource(options.Name, histogram, options.MeasurementUnit, options.Tags));
            });
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric
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

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric
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
            private readonly ConcurrentDictionary<string, MetricMeta> _metrics =
                new ConcurrentDictionary<string, MetricMeta>();

            public IEnumerable<TValue> All
            {
                get { return _metrics.Values.OrderBy(m => m.Name).Select(v => v.Value); }
            }

            public void Clear()
            {
                var values = _metrics.Values;
                _metrics.Clear();
                foreach (var value in values)
                {
                    using (value.Metric as IDisposable)
                    {
                    }
                }
            }

            public TMetric GetOrAdd(string name, Func<Tuple<TMetric, TValue>> metricProvider)
            {
                return _metrics.GetOrAdd(name, n =>
                {
                    var result = metricProvider();
                    return new MetricMeta(result.Item1, result.Item2);
                }).Metric;
            }

            public void Reset()
            {
                foreach (var metric in _metrics.Values)
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