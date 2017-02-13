// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Gauge
{
    /// <summary>
    ///     Combines the value of a gauge (a double) with the defined unit for the value.
    /// </summary>
    public sealed class GaugeValueSource : MetricValueSourceBase<double>
    {
        public GaugeValueSource(
            string name,
            string group,
            IMetricValueProvider<double> value,
            Unit unit,
            MetricTags tags)
            : base(name, group, value, unit, tags)
        {
        }

        public GaugeValueSource(
            string name,
            IMetricValueProvider<double> value,
            Unit unit,
            MetricTags tags)
            : base(name, value, unit, tags)
        {
        }
    }
}