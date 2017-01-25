// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Interfaces;
using App.Metrics.Counter;
using App.Metrics.Counter.Interfaces;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Interfaces;
using App.Metrics.Meter;
using App.Metrics.Meter.Interfaces;
using App.Metrics.Timer;
using App.Metrics.Timer.Interfaces;

namespace App.Metrics.Core
{
    public static class ValueReader
    {
        private static readonly ApdexValue EmptyApdex = new ApdexValue(0.0, 0, 0, 0, 0);
        private static readonly CounterValue EmptyCounter = new CounterValue(0);

        private static readonly HistogramValue EmptyHistogram = new HistogramValue(
            0,
            0.0,
            null,
            0.0,
            null,
            0.0,
            0.0,
            null,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0);

        private static readonly MeterValue EmptyMeter = new MeterValue(0, 0.0, 0.0, 0.0, 0.0, TimeUnit.Seconds);
        private static readonly TimerValue EmptyTimer = new TimerValue(EmptyMeter, EmptyHistogram, 0, 0, TimeUnit.Milliseconds);

        public static CounterValue GetCurrentValue(ICounter metric)
        {
            var implementation = metric as ICounterMetric;
            return implementation?.Value ?? EmptyCounter;
        }

        public static MeterValue GetCurrentValue(IMeter metric)
        {
            var implementation = metric as IMeterMetric;
            return implementation == null ? EmptyMeter : implementation.Value;
        }

        public static HistogramValue GetCurrentValue(IHistogram metric)
        {
            var implementation = metric as IHistogramMetric;
            return implementation != null ? implementation.Value : EmptyHistogram;
        }

        public static TimerValue GetCurrentValue(ITimer metric)
        {
            var implementation = metric as ITimerMetric;
            return implementation != null ? implementation.Value : EmptyTimer;
        }

        public static ApdexValue GetCurrentValue(IApdex metric)
        {
            var implementation = metric as IApdexMetric;
            return implementation != null ? implementation.Value : EmptyApdex;
        }
    }
}