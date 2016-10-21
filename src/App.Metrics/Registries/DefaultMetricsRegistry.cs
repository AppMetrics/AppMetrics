// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;

namespace App.Metrics.Registries
{
    internal sealed class DefaultMetricsRegistry : IMetricsRegistry
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

        public DefaultMetricsRegistry()
        {
            DataProvider = new DefaultMetricRegistryManager(() => _gauges.All, () => _counters.All, () => _meters.All,
                () => _histograms.All, () => _timers.All);
        }

        public IMetricRegistryManager DataProvider { get; }

        public void ClearAllMetrics()
        {
            _gauges.Clear();
            _counters.Clear();
            _meters.Clear();
            _histograms.Clear();
            _timers.Clear();
        }

        public ICounter Counter<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : ICounterImplementation
        {
            return _counters.GetOrAdd(name, () =>
            {
                var counter = builder();
                return Tuple.Create((ICounter)counter, new CounterValueSource(name, counter, unit, tags));
            });
        }

        public void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
            _gauges.GetOrAdd(name, () =>
            {
                var gauge = valueProvider();
                return Tuple.Create(gauge, new GaugeValueSource(name, gauge, unit, tags));
            });
        }

        public IHistogram Histogram<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : IHistogramImplementation
        {
            return _histograms.GetOrAdd(name, () =>
            {
                var histogram = builder();
                return Tuple.Create((IHistogram)histogram, new HistogramValueSource(name, histogram, unit, tags));
            });
        }

        public IMeter Meter<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation
        {
            return _meters.GetOrAdd(name, () =>
            {
                var meter = builder();
                return Tuple.Create((IMeter)meter, new MeterValueSource(name, meter, unit, rateUnit, tags));
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

        public ITimer Timer<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation
        {
            return _timers.GetOrAdd(name, () =>
            {
                var timer = builder();
                return Tuple.Create((ITimer)timer, new TimerValueSource(name, timer, unit, rateUnit, durationUnit, tags));
            });
        }

        #region new contract

        public ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterImplementation
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

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramImplementation
        {
            return _histograms.GetOrAdd(options.Name, () =>
            {
                var histogram = builder();
                return Tuple.Create((IHistogram)histogram, new HistogramValueSource(options.Name, histogram, options.MeasurementUnit, options.Tags));
            });
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterImplementation
        {
            return _meters.GetOrAdd(options.Name, () =>
            {
                var meter = builder();
                return Tuple.Create((IMeter)meter, new MeterValueSource(options.Name, meter, options.MeasurementUnit, options.RateUnit, options.Tags));
            });
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder)
           where T : ITimerImplementation
        {
            return _timers.GetOrAdd(options.Name, () =>
            {
                var timer = builder();
                return Tuple.Create((ITimer)timer,
                    new TimerValueSource(options.Name, timer, options.MeasurementUnit, options.RateUnit, options.DurationUnit, options.Tags));
            });
        }

        #endregion

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