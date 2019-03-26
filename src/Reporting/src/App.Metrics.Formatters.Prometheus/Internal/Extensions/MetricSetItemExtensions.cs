// <copyright file="MetricSetItemExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Counter;
using App.Metrics.Meter;

namespace App.Metrics.Formatters.Prometheus.Internal.Extensions
{
    public static class MetricSetItemExtensions
    {
        public static Metric ToPrometheusMetric(this CounterValue.SetItem item)
        {
            var result = new Metric
                         {
                             gauge = new Gauge
                                     {
                                         value = item.Count
                                     },
                             label = item.Tags.ToLabelPairs()
                         };

            return result;
        }

        public static Metric ToPrometheusMetric(this MeterValue.SetItem item)
        {
            var result = new Metric
                         {
                             counter = new Counter
                                       {
                                           value = item.Value.Count
                                       },
                             label = item.Tags.ToLabelPairs()
                         };

            return result;
        }
    }
}