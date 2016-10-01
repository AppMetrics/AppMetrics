using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsRegistry : MetricsRegistry
    {
        private readonly MetricMetaCatalog<Counter, CounterValueSource, CounterValue> _counters =
            new MetricMetaCatalog<Counter, CounterValueSource, CounterValue>();

        private readonly MetricMetaCatalog<MetricValueProvider<double>, GaugeValueSource, double> _gauges =
            new MetricMetaCatalog<MetricValueProvider<double>, GaugeValueSource, double>();

        private readonly MetricMetaCatalog<Histogram, HistogramValueSource, HistogramValue> _histograms =
            new MetricMetaCatalog<Histogram, HistogramValueSource, HistogramValue>();

        private readonly MetricMetaCatalog<Meter, MeterValueSource, MeterValue> _meters = new MetricMetaCatalog<Meter, MeterValueSource, MeterValue>();
        private readonly MetricMetaCatalog<Timer, TimerValueSource, TimerValue> _timers = new MetricMetaCatalog<Timer, TimerValueSource, TimerValue>();

        public DefaultMetricsRegistry()
        {
            DataProvider = new DefaultRegistryDataProvider(() => _gauges.All, () => _counters.All, () => _meters.All,
                () => _histograms.All, () => _timers.All);
        }

        public RegistryDataProvider DataProvider { get; }

        public void ClearAllMetrics()
        {
            _gauges.Clear();
            _counters.Clear();
            _meters.Clear();
            _histograms.Clear();
            _timers.Clear();
        }

        public Counter Counter<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : CounterImplementation
        {
            return _counters.GetOrAdd(name, () =>
            {
                var counter = builder();
                return Tuple.Create((Counter)counter, new CounterValueSource(name, counter, unit, tags));
            });
        }

        public void Gauge(string name, Func<MetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
            _gauges.GetOrAdd(name, () =>
            {
                var gauge = valueProvider();
                return Tuple.Create(gauge, new GaugeValueSource(name, gauge, unit, tags));
            });
        }

        public Histogram Histogram<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : HistogramImplementation
        {
            return _histograms.GetOrAdd(name, () =>
            {
                var histogram = builder();
                return Tuple.Create((Histogram)histogram, new HistogramValueSource(name, histogram, unit, tags));
            });
        }

        public Meter Meter<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, MetricTags tags)
            where T : MeterImplementation
        {
            return _meters.GetOrAdd(name, () =>
            {
                var meter = builder();
                return Tuple.Create((Meter)meter, new MeterValueSource(name, meter, unit, rateUnit, tags));
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

        public Timer Timer<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : TimerImplementation
        {
            return _timers.GetOrAdd(name, () =>
            {
                var timer = builder();
                return Tuple.Create((Timer)timer, new TimerValueSource(name, timer, unit, rateUnit, durationUnit, tags));
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
                    var resetable = metric.Metric as ResetableMetric;
                    resetable?.Reset();
                }
            }

            public class MetricMeta
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