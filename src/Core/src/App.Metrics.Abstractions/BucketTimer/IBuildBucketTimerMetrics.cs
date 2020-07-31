// <copyright file="IBuildTimerMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.BucketHistogram;
using App.Metrics.Meter;

namespace App.Metrics.BucketTimer
{
    public interface IBuildBucketTimerMetrics
    {
        IBucketTimerMetric Build(IBucketHistogramMetric histogram, IClock clock, TimeUnit timeUnit);

        IBucketTimerMetric Build(IEnumerable<double> buckets, IClock clock, TimeUnit timeUnit);

        IBucketTimerMetric Build(IBucketHistogramMetric histogram, IMeterMetric meter, IClock clock, TimeUnit timeUnit);

        IBucketTimerMetric Build(IEnumerable<double> buckets, IMeterMetric meter, IClock clock, TimeUnit timeUnit);
    }
}