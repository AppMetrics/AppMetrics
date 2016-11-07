// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using App.Metrics.Core;

namespace App.Metrics.Data
{
    public static class ValueReader
    {
        private static readonly CounterValue EmptyCounter = new CounterValue(0);

        private static readonly HistogramValue EmptyHistogram = new HistogramValue(0, 0.0, null, 0.0, null, 0.0, 0.0, null, 0.0, 0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0);

        private static readonly MeterValue EmptyMeter = new MeterValue(0, 0.0, 0.0, 0.0, 0.0, TimeUnit.Seconds);
        private static readonly TimerValue EmptyTimer = new TimerValue(EmptyMeter, EmptyHistogram, 0, 0, TimeUnit.Milliseconds);

        public static CounterValue GetCurrentValue(ICounter metric)
        {
            var implementation = metric as ICounterMetric;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyCounter;
        }

        public static MeterValue GetCurrentValue(IMeter metric)
        {
            var implementation = metric as IMeterMetric;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyMeter;
        }

        public static HistogramValue GetCurrentValue(IHistogram metric)
        {
            var implementation = metric as IHistogramMetric;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyHistogram;
        }

        public static TimerValue GetCurrentValue(ITimer metric)
        {
            var implementation = metric as ITimerMetric;
            if (implementation != null)
            {
                return implementation.Value;
            }
            return EmptyTimer;
        }
    }
}