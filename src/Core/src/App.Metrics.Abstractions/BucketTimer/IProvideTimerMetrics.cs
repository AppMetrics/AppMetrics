// <copyright file="IProvideBucketTimerMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.BucketHistogram;
using App.Metrics.Timer;

namespace App.Metrics.BucketTimer
{
    public interface IProvideBucketTimerMetrics
    {
        ITimer Instance(BucketTimerOptions options);

        ITimer Instance(BucketTimerOptions options, MetricTags tags);

        ITimer Instance<T>(BucketTimerOptions options, Func<T> builder)
            where T : IBucketTimerMetric;

        ITimer Instance<T>(BucketTimerOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketTimerMetric;

        ITimer WithHistogram<T>(BucketTimerOptions options, Func<T> histogramMetricBuilder)
            where T : IBucketHistogramMetric;

        ITimer WithHistogram<T>(BucketTimerOptions options, MetricTags tags, Func<T> histogramMetricBuilder)
            where T : IBucketHistogramMetric;
    }
}