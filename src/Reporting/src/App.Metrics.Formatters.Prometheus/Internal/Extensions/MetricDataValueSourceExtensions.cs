// <copyright file="MetricDataValueSourceExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Formatters.Prometheus.Internal.Extensions
{
    public static class MetricDataValueSourceExtensions
    {
        public static IEnumerable<MetricFamily> GetPrometheusMetricsSnapshot(
            this MetricsDataValueSource snapshot,
            Func<string, string, string> metricNameFormatter,
            Func<string, string> labelNameFormatter)
        {
            var result = new List<MetricFamily>();
            foreach (var group in snapshot.Contexts)
            {
                foreach (var metricGroup in group.ApdexScores.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = metricNameFormatter(group.Context, metricGroup.Key),
                                               type = MetricType.GAUGE
                                           };
                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Gauges.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = metricNameFormatter(group.Context, metricGroup.Key),
                                               type = MetricType.GAUGE
                                           };
                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Counters.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = metricNameFormatter(group.Context, metricGroup.Key),
                                               type = MetricType.GAUGE
                                           };

                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Meters.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = metricNameFormatter(group.Context, $"{metricGroup.Key}_total"),
                                               type = MetricType.COUNTER
                                           };

                    foreach (var metric in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(metric.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Histograms.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = metricNameFormatter(group.Context, metricGroup.Key),
                                               type = MetricType.SUMMARY
                                           };

                    foreach (var timer in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(timer.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.BucketHistograms.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                    {
                        name = metricNameFormatter(group.Context, metricGroup.Key),
                        type = MetricType.HISTOGRAM
                    };

                    foreach (var timer in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(timer.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.Timers.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                                           {
                                               name = metricNameFormatter(group.Context, metricGroup.Key),
                                               type = MetricType.SUMMARY
                                           };

                    foreach (var timer in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(timer.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }

                foreach (var metricGroup in group.BucketTimers.GroupBy(
                    source => source.IsMultidimensional ? source.MultidimensionalName : source.Name))
                {
                    var promMetricFamily = new MetricFamily
                    {
                        name = metricNameFormatter(group.Context, metricGroup.Key),
                        type = MetricType.HISTOGRAM
                    };

                    foreach (var timer in metricGroup)
                    {
                        promMetricFamily.metric.AddRange(timer.ToPrometheusMetrics(labelNameFormatter));
                    }

                    result.Add(promMetricFamily);
                }
            }

            return result;
        }
    }
}