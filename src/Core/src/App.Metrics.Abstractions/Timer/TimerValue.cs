// <copyright file="TimerValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace App.Metrics.Timer
{
    /// <summary>
    ///     The value reported by a Timer Metric
    /// </summary>
    public sealed class TimerValue
    {
        public TimerValue(MeterValue rate, HistogramValue histogram, long activeSessions, TimeUnit durationUnit)
        {
            Rate = rate;
            Histogram = histogram;
            ActiveSessions = activeSessions;
            DurationUnit = durationUnit;
        }

        public long ActiveSessions { get; }

        public HistogramValue Histogram { get; }

        public MeterValue Rate { get; }

        public TimeUnit DurationUnit { get; }

        public TimerValue Scale(TimeUnit rate, TimeUnit duration)
        {
            var durationFactor = DurationUnit.ScalingFactorFor(duration);
            return new TimerValue(Rate.Scale(rate), Histogram.Scale(durationFactor), ActiveSessions, duration);
        }
    }
}