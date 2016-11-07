// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


namespace App.Metrics.Data
{
    /// <summary>
    ///     The value reported by a Timer Metric
    /// </summary>
    public sealed class TimerValue
    {
        public readonly long ActiveSessions;
        public readonly HistogramValue Histogram;
        public readonly MeterValue Rate;
        public readonly long TotalTime;
        private readonly TimeUnit _durationUnit;

        public TimerValue(MeterValue rate, HistogramValue histogram, long activeSessions, long totalTime, TimeUnit durationUnit)
        {
            Rate = rate;
            Histogram = histogram;
            ActiveSessions = activeSessions;
            TotalTime = totalTime;
            _durationUnit = durationUnit;
        }

        public TimerValue Scale(TimeUnit rate, TimeUnit duration)
        {
            var durationFactor = _durationUnit.ScalingFactorFor(duration);
            var total = _durationUnit.Convert(duration, TotalTime);
            return new TimerValue(Rate.Scale(rate), Histogram.Scale(durationFactor), ActiveSessions, total, duration);
        }
    }

    /// <summary>
    ///     Combines the value of the timer with the defined unit and the time units for rate and duration.
    /// </summary>
    public sealed class TimerValueSource : MetricValueSource<TimerValue>
    {
        public TimerValueSource(string name, IMetricValueProvider<TimerValue> value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit,
            MetricTags tags)
            : base(name, new ScaledValueProvider<TimerValue>(value, v => v.Scale(rateUnit, durationUnit)), unit, tags)
        {
            RateUnit = rateUnit;
            DurationUnit = durationUnit;
        }

        public TimeUnit DurationUnit { get; private set; }

        public TimeUnit RateUnit { get; private set; }
    }
}