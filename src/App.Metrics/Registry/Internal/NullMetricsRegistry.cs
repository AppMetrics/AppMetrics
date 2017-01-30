// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Internal;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NullMetricsRegistry : IMetricsRegistry
    {
        public static void Reset() { }

        public IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric { return _apdexInstance; }

        public void Clear() { }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric { return _counterInstance; }

        /// <inheritdoc />
        public void Disable() { }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider) { }

        public MetricsDataValueSource GetData(IFilterMetrics filter) { return MetricsDataValueSource.Empty; }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric { return _histogramInstance; }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric { return _meterInstance; }

        public void RemoveContext(string context) { }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric { return _timerInstance; }

#pragma warning disable SA1129
        private readonly IApdex _apdexInstance = new NullApdex();
        private readonly ICounter _counterInstance = new NullCounter();
        private readonly IHistogram _histogramInstance = new NullHistogram();
        private readonly IMeter _meterInstance = new NullMeter();
        private readonly ITimer _timerInstance = new NullTimer();
#pragma warning restore SA1129
    }
}