// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
            return valueService.GetForContext(context).Meters.ValueFor(context, metricName);
        }

        public static MeterValue GetValueOrDefault(this IMeter metric)
        {
            var implementation = metric as IMeterMetric;
            return implementation == null ? EmptyMeter : implementation.Value;
        }
    }
}