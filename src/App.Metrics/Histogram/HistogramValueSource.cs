// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

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

        public HistogramValueSource(
            string name,
            string group,
            IMetricValueProvider<HistogramValue> valueProvider,
            Unit unit,
            MetricTags tags)
            : base(name, group, valueProvider, unit, tags)
        {
        }
    }
}