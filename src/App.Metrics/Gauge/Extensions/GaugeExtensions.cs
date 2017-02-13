// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Core.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Gauge
    // ReSharper restore CheckNamespace
{
    public static class GaugeExtensions
    {
        public static double GetGaugeValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Gauges.ValueFor(context, metricName);
        }
    }
}