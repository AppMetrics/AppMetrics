// <copyright file="HistogramMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     <para>
    ///         Bucket Histogram metric types track the count of a set of values. They allow you to measure the
    ///         count of values per bucket
    ///     </para>
    /// </summary>
    public sealed class BucketHistogramMetric : MetricBase
    {
        public long Count { get; set; }

        public double Sum { get; set; }

        public IDictionary<double, double> Buckets { get; set; }
    }
}