// <copyright file="HistogramValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Histogram
{
    /// <summary>
    ///     Combines the value of the histogram with the defined unit for the value.
    /// </summary>
    public sealed class HistogramValueSource : MetricValueSourceBase<HistogramValue>
    {
        public HistogramValueSource(
            string name,
            IMetricValueProvider<HistogramValue> valueProvider,
            Unit unit,
            MetricTags tags,
            bool restOnReporting = false)
            : base(name, valueProvider, unit, tags, restOnReporting)
        {
        }
    }
}
