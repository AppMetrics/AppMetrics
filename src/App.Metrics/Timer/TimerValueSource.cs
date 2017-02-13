// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Timer
{
    /// <summary>
    ///     Combines the value of the timer with the defined unit and the time units for rate and duration.
    /// </summary>
    public sealed class TimerValueSource : MetricValueSourceBase<TimerValue>
    {
        public TimerValueSource(
            string name,
            IMetricValueProvider<TimerValue> value,
            Unit unit,
            TimeUnit rateUnit,
            TimeUnit durationUnit,
            MetricTags tags)
            : base(name, new ScaledValueProvider<TimerValue>(value, v => v.Scale(rateUnit, durationUnit)), unit, tags)
        {
            RateUnit = rateUnit;
            DurationUnit = durationUnit;
        }

        public TimerValueSource(
            string name,
            string group,
            IMetricValueProvider<TimerValue> value,
            Unit unit,
            TimeUnit rateUnit,
            TimeUnit durationUnit,
            MetricTags tags)
            : base(name, group, new ScaledValueProvider<TimerValue>(value, v => v.Scale(rateUnit, durationUnit)), unit, tags)
        {
            RateUnit = rateUnit;
            DurationUnit = durationUnit;
        }

        public TimeUnit DurationUnit { get; private set; }

        public TimeUnit RateUnit { get; private set; }
    }
}