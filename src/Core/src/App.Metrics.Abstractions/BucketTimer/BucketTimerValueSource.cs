// <copyright file="TimerValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.BucketTimer
{
    /// <summary>
    ///     Combines the value of the timer with the defined unit and the time units for rate and duration.
    /// </summary>
    public sealed class BucketTimerValueSource : MetricValueSourceBase<BucketTimerValue>
    {
        public BucketTimerValueSource(
            string name,
            IMetricValueProvider<BucketTimerValue> value,
            Unit unit,
            TimeUnit rateUnit,
            TimeUnit durationUnit,
            MetricTags tags)
            : base(name, new ScaledValueProvider<BucketTimerValue>(value, v => v.Scale(rateUnit, durationUnit)), unit, tags)
        {
            DurationUnit = durationUnit;
            RateUnit = rateUnit;
        }

        public TimeUnit DurationUnit { get; }

        public TimeUnit RateUnit { get; }
    }
}