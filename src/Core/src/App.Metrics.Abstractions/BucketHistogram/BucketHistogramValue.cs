// <copyright file="HistogramValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.BucketHistogram
{
    /// <summary>
    ///     The value reported by a Histogram Metric
    /// </summary>
    public sealed class BucketHistogramValue
    {
        public BucketHistogramValue(
            long count,
            double sum,
            IReadOnlyDictionary<long, long> buckets)
        {
            Count = count;
            Sum = sum;
            Buckets = buckets;
        }

        public long Count { get; }

        public double Sum { get; }

        public IReadOnlyDictionary<long, long> Buckets { get; }
    }
}