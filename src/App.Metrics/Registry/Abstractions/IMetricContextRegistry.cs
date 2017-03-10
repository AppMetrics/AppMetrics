// <copyright file="IMetricContextRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Tagging;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Registry.Abstractions
{
    public interface IMetricContextRegistry
    {
        string Context { get; }

        IMetricRegistryManager DataProvider { get; }

        IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric;

        IApdex Apdex<T>(ApdexOptions options, MetricTags tags, Func<T> builder)
            where T : IApdexMetric;

        void ClearAllMetrics();

        ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric;

        ICounter Counter<T>(CounterOptions options, MetricTags tags, Func<T> builder)
            where T : ICounterMetric;

        IGauge Gauge<T>(GaugeOptions options, Func<T> builder)
            where T : IGaugeMetric;

        IGauge Gauge<T>(GaugeOptions options, MetricTags tags, Func<T> builder)
            where T : IGaugeMetric;

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric;

        IHistogram Histogram<T>(HistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IHistogramMetric;

        IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric;

        IMeter Meter<T>(MeterOptions options, MetricTags tags, Func<T> builder)
            where T : IMeterMetric;

        ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric;

        ITimer Timer<T>(TimerOptions options, MetricTags tags, Func<T> builder)
            where T : ITimerMetric;
    }
}