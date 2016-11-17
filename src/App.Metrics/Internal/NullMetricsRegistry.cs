// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Data;
using App.Metrics.Data.Interfaces;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal
{
    internal sealed class NullMetricsRegistry : IMetricsRegistry
    {
        public bool AddContext(string context, IMetricContextRegistry registry)
        {
            return true;
        }

        public void Clear()
        {
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric
        {
            return NullMetric.Instance;
        }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
        {
        }

        public Task<MetricsDataValueSource> GetDataAsync(IMetricsFilter filter)
        {
            return AppMetricsTaskCache.EmptyMetricsDataTask;
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric
        {
            return NullMetric.Instance;
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric
        {
            return NullMetric.Instance;
        }

        public void RemoveContext(string context)
        {
        }

        public void Reset()
        {
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric
        {
            return NullMetric.Instance;
        }

        private struct NullMetric : ICounter, IMeter, IHistogram, ITimer, IMetricRegistryManager
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

            public void Mark(MetricItem item, long amount)
            {
            }

            public void Mark(long amount)
            {
            }

            public void Mark(string item)
            {
            }

            public void Mark(MetricItem item)
            {
            }

            public void Mark(string item, long amount)
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

            public void Decrement(MetricItem item)
            {
            }

            public void Decrement(MetricItem item, long amount)
            {
            }

            public void Increment(MetricItem item)
            {
            }

            public void Increment(MetricItem item, long amount)
            {
            }
        }
    }
}