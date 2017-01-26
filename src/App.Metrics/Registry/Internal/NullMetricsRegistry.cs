// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Internal;
using App.Metrics.Core.Options;
using App.Metrics.Counter;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NullMetricsRegistry : IMetricsRegistry
    {
        public bool AddContext(string context, IMetricContextRegistry registry) { return true; }

        public IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric { return NullMetric.Instance; }

        public void Clear() { }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric { return NullMetric.Instance; }

        /// <inheritdoc />
        public void Disable() { }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider) { }

        public MetricsDataValueSource GetData(IFilterMetrics filter) { return MetricsDataValueSource.Empty; }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric { return NullMetric.Instance; }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric { return NullMetric.Instance; }

        public void RemoveContext(string context) { }

        public void Reset() { }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric { return NullMetric.Instance; }

        private struct NullMetric : IApdex, ICounter, IMeter, IHistogram, ITimer, IMetricRegistryManager
        {
            public static readonly NullMetric Instance = default(NullMetric);
            private static readonly ApdexContext NullApdexContextContext = new ApdexContext(Instance);
            private static readonly TimerContext NullContext = new TimerContext(Instance, null);

            public IEnumerable<ApdexValueSource> ApdexScores
            {
                get { yield break; }
            }

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

            public long CurrentTime() { return 0; }

            public void Decrement() { }

            public void Decrement(long value) { }

            public void Decrement(string item) { }

            public void Decrement(string item, long value) { }

            public void Decrement(MetricItem item) { }

            public void Decrement(MetricItem item, long amount) { }

            public long EndRecording() { return 0; }

            public void Increment() { }

            public void Increment(long value) { }

            public void Increment(string item) { }

            public void Increment(string item, long value) { }

            public void Increment(MetricItem item) { }

            public void Increment(MetricItem item, long amount) { }

            public void Mark() { }

            public void Mark(MetricItem item, long amount) { }

            public void Mark(long amount) { }

            public void Mark(string item) { }

            public void Mark(MetricItem item) { }

            public void Mark(string item, long amount) { }

            public TimerContext NewContext(string userValue) { return NullContext; }

            public ApdexContext NewContext() { return NullApdexContextContext; }

            public void Record(long time, TimeUnit unit, string userValue) { }

            /// <inheritdoc />
            public void Record(long time, TimeUnit unit) { }

            public void Reset() { }

            public long StartRecording() { return 0; }

            public void Time(Action action, string userValue) { action(); }

            public T Time<T>(Func<T> action, string userValue) { return action(); }

            /// <inheritdoc />
            public void Time(Action action) { }

            /// <inheritdoc />
            public T Time<T>(Func<T> action) { return action(); }

            public void Track(long duration) { }

            public void Track(Action action) { }

            public T Track<T>(Func<T> action) { return action(); }

            public void Update(long value, string userValue) { }

            /// <inheritdoc />
            public void Update(long value) { }

            /// <inheritdoc />
            TimerContext ITimer.NewContext() { return NullContext; }
        }
    }
}