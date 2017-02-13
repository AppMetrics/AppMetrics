// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

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

        public ApdexValueSource(
            string name,
            string group,
            IMetricValueProvider<ApdexValue> value,
            MetricTags tags,
            bool resetOnReporting = false)
            : base(name, group, value, Unit.Results, tags)
        {
            ResetOnReporting = resetOnReporting;
        }

        public bool ResetOnReporting { get; private set; }
    }
}