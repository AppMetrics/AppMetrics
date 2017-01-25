// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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

        public TimerValue(MeterValue rate, HistogramValue histogram, long activeSessions, long totalTime, TimeUnit durationUnit)
        {
            Rate = rate;
            Histogram = histogram;
            ActiveSessions = activeSessions;
            TotalTime = totalTime;
            _durationUnit = durationUnit;
        }

        public long ActiveSessions { get; }

        public HistogramValue Histogram { get; }

        public MeterValue Rate { get; }

        public long TotalTime { get; }

        public TimerValue Scale(TimeUnit rate, TimeUnit duration)
        {
            var durationFactor = _durationUnit.ScalingFactorFor(duration);
            var total = _durationUnit.Convert(duration, TotalTime);
            return new TimerValue(Rate.Scale(rate), Histogram.Scale(durationFactor), ActiveSessions, total, duration);
        }
    }
}