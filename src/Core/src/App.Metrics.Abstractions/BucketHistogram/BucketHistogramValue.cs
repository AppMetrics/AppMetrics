// <copyright file="HistogramValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Histogram;

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
            IReadOnlyDictionary<double, double> buckets)
        {
            Count = count;
            Sum = sum;
            Buckets = buckets;
        }

        public long Count { get; }

        public double Sum { get; }

        public IReadOnlyDictionary<double, double> Buckets { get; }

        public BucketHistogramValue Scale(double factor)
        {
            if (Math.Abs(factor - 1.0d) < 0.001)
            {
                return this;
            }

            return new BucketHistogramValue(
                Count,
                Sum * factor,
                Buckets.ToDictionary(x=>x.Key, x => x.Value * factor));
        }
    }
}