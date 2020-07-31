// <copyright file="HistogramOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.BucketHistogram
{
    /// <summary>
    ///     Configuration of an <see cref="IBucketHistogramMetric" /> that will be measured
    /// </summary>
    /// <seealso cref="MetricValueOptionsBase" />
    public class BucketHistogramOptions : MetricValueOptionsBase
    {
        public IEnumerable<double> Buckets { get; set; }
    }
}