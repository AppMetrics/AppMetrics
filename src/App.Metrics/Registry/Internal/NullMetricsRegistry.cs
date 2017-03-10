// <copyright file="NullMetricsRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core;
using App.Metrics.Core.Internal;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NullMetricsRegistry : IMetricsRegistry
    {
        public static void Reset() { }

        public IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric
        {
            return _apdexInstance;
        }

        /// <inheritdoc />
        public IApdex Apdex<T>(ApdexOptions options, MetricTags tags, Func<T> builder)
            where T : IApdexMetric
        {
            return _apdexInstance;
        }

        public void Clear() { }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric
        {
            return _counterInstance;
        }

        /// <inheritdoc />
        public ICounter Counter<T>(CounterOptions options, MetricTags tags, Func<T> builder)
            where T : ICounterMetric
        {
            return _counterInstance;
        }

        /// <inheritdoc />
        public void Disable()
        {
        }

        /// <inheritdoc />
        public IGauge Gauge<T>(GaugeOptions options, Func<T> builder)
            where T : IGaugeMetric
        {
            return _gaugeInstance;
        }

        /// <inheritdoc />
        public IGauge Gauge<T>(GaugeOptions options, MetricTags tags, Func<T> builder)
            where T : IGaugeMetric
        {
            return _gaugeInstance;
        }

        public MetricsDataValueSource GetData(IFilterMetrics filter) { return MetricsDataValueSource.Empty; }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric
        {
            return _histogramInstance;
        }

        /// <inheritdoc />
        public IHistogram Histogram<T>(HistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IHistogramMetric
        {
            return _histogramInstance;
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric
        {
            return _meterInstance;
        }

        /// <inheritdoc />
        public IMeter Meter<T>(MeterOptions options, MetricTags tags, Func<T> builder)
            where T : IMeterMetric
        {
            return _meterInstance;
        }

        public void RemoveContext(string context) { }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric
        {
            return _timerInstance;
        }

        /// <inheritdoc />
        public ITimer Timer<T>(TimerOptions options, MetricTags tags, Func<T> builder)
            where T : ITimerMetric
        {
            return _timerInstance;
        }

#pragma warning disable SA1129
        private readonly IApdex _apdexInstance = new NullApdex();
        private readonly ICounter _counterInstance = new NullCounter();
        private readonly IGauge _gaugeInstance = new NullGauge();
        private readonly IHistogram _histogramInstance = new NullHistogram();
        private readonly IMeter _meterInstance = new NullMeter();
        private readonly ITimer _timerInstance = new NullTimer();
#pragma warning restore SA1129
    }
}