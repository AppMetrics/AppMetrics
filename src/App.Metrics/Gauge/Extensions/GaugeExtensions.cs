// <copyright file="GaugeExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Tagging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Gauge
    // ReSharper restore CheckNamespace
{
    public static class GaugeExtensions
    {
        public static double GetGaugeValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Gauges.ValueFor(metricName);
        }

        public static double GetGaugeValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).Gauges.ValueFor(tags.AsMetricName(metricName));
        }
    }
}