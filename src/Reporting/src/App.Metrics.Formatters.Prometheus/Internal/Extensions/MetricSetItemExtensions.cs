// <copyright file="MetricSetItemExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Counter;
using App.Metrics.Meter;

namespace App.Metrics.Formatters.Prometheus.Internal.Extensions
{
    public static class MetricSetItemExtensions
    {
        public static Metric ToPrometheusMetric(
            this CounterValue.SetItem item,
            List<LabelPair> parentTags,
            Func<string, string> labelNameFormatter)
        {
            var tags = item.Tags.ToLabelPairs(labelNameFormatter);
            tags.AddRange(parentTags);
            var result = new Metric
                         {
                             gauge = new Gauge
                                     {
                                         value = item.Count
                                     },
                             label = tags
                         };

            return result;
        }

        public static Metric ToPrometheusMetric(
            this MeterValue.SetItem item,
            List<LabelPair> parentTags,
            Func<string, string> labelNameFormatter)
        {
            var tags = item.Tags.ToLabelPairs(labelNameFormatter);
            tags.AddRange(parentTags);
            var result = new Metric
                         {
                             counter = new Counter
                                       {
                                           value = item.Value.Count
                                       },
                             label = tags
                         };

            return result;
        }
    }
}