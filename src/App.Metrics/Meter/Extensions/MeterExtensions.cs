// <copyright file="MeterExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Meter.Abstractions;

namespace App.Metrics.Meter.Extensions
{
    public static class MeterExtensions
    {
        private static readonly MeterValue EmptyMeter = new MeterValue(0, 0.0, 0.0, 0.0, 0.0, TimeUnit.Seconds);

        public static MeterValue GetMeterValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Meters.ValueFor(metricName);
        }

        public static MeterValue GetValueOrDefault(this IMeter metric)
        {
            var implementation = metric as IMeterMetric;
            return implementation == null ? EmptyMeter : implementation.Value;
        }
    }
}