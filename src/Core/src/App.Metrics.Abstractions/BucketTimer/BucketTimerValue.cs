// <copyright file="TimerValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.BucketHistogram;
using App.Metrics.Meter;

namespace App.Metrics.BucketTimer
{
    /// <summary>
    ///     The value reported by a Timer Metric
    /// </summary>
    public sealed class BucketTimerValue
    {
        public BucketTimerValue(MeterValue rate, BucketHistogramValue histogram, long activeSessions, TimeUnit durationUnit)
        {
            Rate = rate;
            Histogram = histogram;
            ActiveSessions = activeSessions;
            DurationUnit = durationUnit;
        }

        public long ActiveSessions { get; }

        public BucketHistogramValue Histogram { get; }

        public MeterValue Rate { get; }

        public TimeUnit DurationUnit { get; }

        public BucketTimerValue Scale(TimeUnit rate, TimeUnit duration)
        {
            var durationFactor = DurationUnit.ScalingFactorFor(duration);
            return new BucketTimerValue(Rate.Scale(rate), Histogram, ActiveSessions, duration);
        }
    }
}