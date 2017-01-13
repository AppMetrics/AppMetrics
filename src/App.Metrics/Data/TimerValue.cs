// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

namespace App.Metrics.Data
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