// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Meter
{
    /// <summary>
    ///     Combines the value of the meter with the defined unit and the rate unit at which the value is reported.
    /// </summary>
    public sealed class MeterValueSource : MetricValueSourceBase<MeterValue>
    {
        public MeterValueSource(
            string name,
            IMetricValueProvider<MeterValue> value,
            Unit unit,
            TimeUnit rateUnit,
            MetricTags tags)
            : base(name, new ScaledValueProvider<MeterValue>(value, v => v.Scale(rateUnit)), unit, tags)
        {
            RateUnit = rateUnit;
        }

        public MeterValueSource(
            string name,
            string group,
            IMetricValueProvider<MeterValue> value,
            Unit unit,
            TimeUnit rateUnit,
            MetricTags tags)
            : base(name, group, new ScaledValueProvider<MeterValue>(value, v => v.Scale(rateUnit)), unit, tags)
        {
            RateUnit = rateUnit;
        }

        public TimeUnit RateUnit { get; private set; }
    }
}