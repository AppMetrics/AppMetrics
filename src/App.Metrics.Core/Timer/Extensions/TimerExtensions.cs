// <copyright file="TimerExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Histogram;
using App.Metrics.Meter;

// ReSharper disable CheckNamespace
namespace App.Metrics.Timer
    // ReSharper restore CheckNamespace
{
    public static class TimerExtensions
    {
        private static readonly HistogramValue EmptyHistogram = new HistogramValue(
            0,
            0.0,
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
        private static readonly TimerValue EmptyTimer = new TimerValue(EmptyMeter, EmptyHistogram, 0, TimeUnit.Milliseconds);

        public static TimerValue GetTimerValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Timers.ValueFor(metricName);
        }

        public static TimerValue GetTimerValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).Timers.ValueFor(tags.AsMetricName(metricName));
        }

        public static TimerValue GetValueOrDefault(this ITimer metric)
        {
            var implementation = metric as ITimerMetric;
            return implementation != null ? implementation.Value : EmptyTimer;
        }
    }
}