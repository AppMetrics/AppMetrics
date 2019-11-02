// <copyright file="ApdexValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Apdex
{
    public sealed class ApdexValueSource : MetricValueSourceBase<ApdexValue>
    {
        public ApdexValueSource(
            string name,
            IMetricValueProvider<ApdexValue> value,
            MetricTags tags,
            bool resetOnReporting = false)
            : base(name, value, Unit.Results, tags, resetOnReporting)
        {}
    }
}
