// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Tagging;

namespace App.Metrics.Apdex
{
    public sealed class ApdexValueSource : MetricValueSource<ApdexValue>
    {
        public ApdexValueSource(
            string name,
            IMetricValueProvider<ApdexValue> value,
            MetricTags tags,
            bool resetOnReporting = false)
            : base(name, value, Unit.Results, tags) { ResetOnReporting = resetOnReporting; }

        public bool ResetOnReporting { get; private set; }
    }
}