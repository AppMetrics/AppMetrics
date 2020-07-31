// <copyright file="HistogramValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.BucketHistogram
{
    /// <summary>
    ///     Combines the value of the histogram with the defined unit for the value.
    /// </summary>
    public sealed class BucketHistogramValueSource : MetricValueSourceBase<BucketHistogramValue>
    {
        public BucketHistogramValueSource(
            string name,
            IMetricValueProvider<BucketHistogramValue> valueProvider,
            Unit unit,
            MetricTags tags)
            : base(name, valueProvider, unit, tags)
        {
        }
    }
}