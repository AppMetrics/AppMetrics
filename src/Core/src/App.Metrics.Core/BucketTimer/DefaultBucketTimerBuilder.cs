// <copyright file="DefaultTimerBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.BucketHistogram;
using App.Metrics.Meter;

namespace App.Metrics.BucketTimer
{
    public class DefaultBucketTimerBuilder : IBuildBucketTimerMetrics
    {
        public DefaultBucketTimerBuilder()
        {
        }

        /// <inheritdoc />
        public IBucketTimerMetric Build(IBucketHistogramMetric histogram, IClock clock)
        {
            return new DefaultBucketTimerMetric(histogram, clock);
        }

        /// <inheritdoc />
        public IBucketTimerMetric Build(IEnumerable<double> buckets, IClock clock)
        {
            return new DefaultBucketTimerMetric(new DefaultBucketHistogramMetric(buckets), clock);
        }

        /// <inheritdoc />
        public IBucketTimerMetric Build(IBucketHistogramMetric histogram, IMeterMetric meter, IClock clock)
        {
            return new DefaultBucketTimerMetric(histogram, meter, clock);
        }

        /// <inheritdoc />
        public IBucketTimerMetric Build(IEnumerable<double> buckets, IMeterMetric meter, IClock clock)
        {
            return new DefaultBucketTimerMetric(new DefaultBucketHistogramMetric(buckets), meter, clock);
        }
    }
}