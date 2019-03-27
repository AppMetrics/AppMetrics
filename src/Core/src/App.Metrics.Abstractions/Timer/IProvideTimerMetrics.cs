// <copyright file="IProvideTimerMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;

namespace App.Metrics.Timer
{
    public interface IProvideTimerMetrics
    {
        ITimer Instance(TimerOptions options);

        ITimer Instance(TimerOptions options, MetricTags tags);

        ITimer Instance<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric;

        ITimer Instance<T>(TimerOptions options, MetricTags tags, Func<T> builder)
            where T : ITimerMetric;

        ITimer WithHistogram<T>(TimerOptions options, Func<T> histogramMetricBuilder)
            where T : IHistogramMetric;

        ITimer WithHistogram<T>(TimerOptions options, MetricTags tags, Func<T> histogramMetricBuilder)
            where T : IHistogramMetric;
    }
}