// <copyright file="IBuildHistogramMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.BucketHistogram
{
    public interface IBuildBucketHistogramMetrics
    {
        IBucketHistogramMetric Build(IEnumerable<double> buckets);
    }
}