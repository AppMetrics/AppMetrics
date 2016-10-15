using System;
using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;

namespace App.Metrics.Registries
{
    public sealed class NullMetricsRegistry : IMetricsRegistry
    {
        public IRegistryDataProvider DataProvider => NullMetric.Instance;

        public void ClearAllMetrics()
        {
        }

        public ICounter Counter<T>(string name, Func<T> builder, Unit unit, MetricTags tags) where T : ICounterImplementation
        {
            return NullMetric.Instance;
        }

        public void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags)
        {
        }

        public IHistogram Histogram<T>(string name, Func<T> builder, Unit unit, MetricTags tags) where T : IHistogramImplementation
        {
            return NullMetric.Instance;
        }

        public IMeter Meter<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, MetricTags tags) where T : IMeterImplementation
        {
            return NullMetric.Instance;
        }

        public void ResetMetricsValues()
        {
        }

        public ITimer Timer<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation
        {
            return NullMetric.Instance;
        }

        private struct NullMetric : ICounter, IMeter, IHistogram, ITimer, IRegistryDataProvider
        {
            public static readonly NullMetric Instance = new NullMetric();
            private static readonly TimerContext NullContext = new TimerContext(Instance, null);

            public IEnumerable<CounterValueSource> Counters
            {
                get { yield break; }
            }

            public IEnumerable<GaugeValueSource> Gauges
            {
                get { yield break; }
            }

            public IEnumerable<HistogramValueSource> Histograms
            {
                get { yield break; }
            }

            public IEnumerable<MeterValueSource> Meters
            {
                get { yield break; }
            }

            public IEnumerable<TimerValueSource> Timers
            {
                get { yield break; }
            }

            public long CurrentTime()
            {
                return 0;
            }

            public void Decrement()
            {
            }

            public void Decrement(long value)
            {
            }

            public void Decrement(string item)
            {
            }

            public void Decrement(string item, long value)
            {
            }

            public long EndRecording()
            {
                return 0;
            }

            public void Increment()
            {
            }

            public void Increment(long value)
            {
            }

            public void Increment(string item)
            {
            }

            public void Increment(string item, long value)
            {
            }

            public void Mark()
            {
            }

            public void Mark(long count)
            {
            }

            public void Mark(string item)
            {
            }

            public void Mark(string item, long count)
            {
            }

            public TimerContext NewContext(string userValue = null)
            {
                return NullContext;
            }

            public void Record(long time, TimeUnit unit, string userValue = null)
            {
            }

            public void Reset()
            {
            }

            public long StartRecording()
            {
                return 0;
            }

            public void Time(Action action, string userValue = null)
            {
                action();
            }

            public T Time<T>(Func<T> action, string userValue = null)
            {
                return action();
            }

            public void Update(long value, string userValue)
            {
            }
        }
    }
}