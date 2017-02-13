// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Timer
    // ReSharper restore CheckNamespace
{
    public static class TimerExtensions
    {
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

        public static TimerValue GetTimerValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Timers.ValueFor(context, metricName);
        }

        public static TimerValue GetValueOrDefault(this ITimer metric)
        {
            var implementation = metric as ITimerMetric;
            return implementation != null ? implementation.Value : EmptyTimer;
        }
    }
}