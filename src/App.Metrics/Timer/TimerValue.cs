// <copyright file="TimerValue.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
        private readonly TimeUnit _durationUnit;

        public TimerValue(MeterValue rate, HistogramValue histogram, long activeSessions, TimeUnit durationUnit)
        {
            Rate = rate;
            Histogram = histogram;
            ActiveSessions = activeSessions;
            _durationUnit = durationUnit;
        }

        public long ActiveSessions { get; }

        public HistogramValue Histogram { get; }

        public MeterValue Rate { get; }

        public TimerValue Scale(TimeUnit rate, TimeUnit duration)
        {
            var durationFactor = _durationUnit.ScalingFactorFor(duration);
            return new TimerValue(Rate.Scale(rate), Histogram.Scale(durationFactor), ActiveSessions, duration);
        }
    }
}