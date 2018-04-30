// <copyright file="MeterExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Meter
    // ReSharper restore CheckNamespace
{
    public static class MeterExtensions
    {
        private static readonly MeterValue EmptyMeter = new MeterValue(0, 0.0, 0.0, 0.0, 0.0, TimeUnit.Seconds);

        public static MeterValue GetMeterValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Meters.ValueFor(metricName);
        }

        public static MeterValue GetMeterValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).Meters.ValueFor(tags.AsMetricName(metricName));
        }

        public static MeterValue GetValueOrDefault(this IMeter metric)
        {
            var implementation = metric as IMeterMetric;
            return implementation == null ? EmptyMeter : implementation.Value;
        }
    }
}