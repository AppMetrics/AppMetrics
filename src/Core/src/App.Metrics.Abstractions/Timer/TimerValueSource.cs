// <copyright file="TimerValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

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
            MetricTags tags,
            bool restOnReporting = false)
            : base(name, new ScaledValueProvider<TimerValue>(value, v => v.Scale(rateUnit, durationUnit)), unit, tags, restOnReporting)
        {
            RateUnit = rateUnit;
            DurationUnit = durationUnit;
        }

        public TimeUnit DurationUnit { get; }

        public TimeUnit RateUnit { get; }
    }
}
