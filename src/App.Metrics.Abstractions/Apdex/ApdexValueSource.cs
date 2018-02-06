// <copyright file="ApdexValueSource.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
            : base(name, value, Unit.Results, tags)
        {
            ResetOnReporting = resetOnReporting;
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private bool ResetOnReporting { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}