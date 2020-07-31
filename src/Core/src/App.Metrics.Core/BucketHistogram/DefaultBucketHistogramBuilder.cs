// <copyright file="DefaultHistogramBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.BucketHistogram
{
    public class DefaultBucketHistogramBuilder : IBuildBucketHistogramMetrics
    {
        /// <inheritdoc />
        public IBucketHistogramMetric Build(IEnumerable<double> buckets)
        {
            return new DefaultBucketHistogramMetric(buckets);
        }
    }
}