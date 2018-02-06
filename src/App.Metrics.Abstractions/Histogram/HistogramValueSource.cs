// <copyright file="HistogramValueSource.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
            MetricTags tags)
            : base(name, valueProvider, unit, tags)
        {
        }
    }
}