// <copyright file="MetricValueSourceExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Formatters.Prometheus.Internal.Extensions
{
    public static class MetricValueSourceExtensions
    {
        public static IEnumerable<Metric> ToPrometheusMetrics(
            this ApdexValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            var result = new List<Metric>
                         {
                             new Metric
                             {
                                 gauge = new Gauge
                                         {
                                             value = metric.Value.Score
                                         },
                                 label = metric.Tags.ToLabelPairs(labelNameFormatter)
                             }
                         };

            return result;
        }

        public static IEnumerable<Metric> ToPrometheusMetrics(
            this GaugeValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            var result = new List<Metric>
                         {
                             new Metric
                             {
                                 gauge = new Gauge
                                         {
                                             value = metric.Value
                                         },
                                 label = metric.Tags.ToLabelPairs(labelNameFormatter)
                             }
                         };

            return result;
        }

        public static IEnumerable<Metric> ToPrometheusMetrics(
            this CounterValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            var tags = metric.Tags.ToLabelPairs(labelNameFormatter);
            var result = new List<Metric>
                         {
                             new Metric
                             {
                                 gauge = new Gauge
                                         {
                                             value = metric.ValueProvider.GetValue(metric.ResetOnReporting).Count
                                         },
                                 label = tags
                             }
                         };

            if (metric.Value.Items?.Length > 0)
            {
                result.AddRange(metric.Value.Items.Select(i => i.ToPrometheusMetric(tags, labelNameFormatter)));
            }

            return result;
        }

        public static IEnumerable<Metric> ToPrometheusMetrics(
            this MeterValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            var tags = metric.Tags.ToLabelPairs(labelNameFormatter);
            var result = new List<Metric>
                         {
                             new Metric
                             {
                                 counter = new Counter
                                           {
                                               value = metric.Value.Count
                                           },
                                 label = tags
                             }
                         };

            if (metric.Value.Items?.Length > 0)
            {
                result.AddRange(metric.Value.Items.Select(x => x.ToPrometheusMetric(tags, labelNameFormatter)));
            }

            return result;
        }

        public static IEnumerable<Metric> ToPrometheusMetrics(
            this HistogramValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            var result = new List<Metric>
                         {
                             new Metric
                             {
                                 summary = new Summary
                                           {
                                               sample_count = (ulong)metric.Value.Count,
                                               sample_sum = metric.Value.Sum,
                                               quantile =
                                               {
                                                   new Quantile { quantile = 0.5, value = metric.Value.Median },
                                                   new Quantile { quantile = 0.75, value = metric.Value.Percentile75 },
                                                   new Quantile { quantile = 0.95, value = metric.Value.Percentile95 },
                                                   // new Quantile(){quantile = 0.98, value = metric.Value.Percentile98},
                                                   new Quantile { quantile = 0.99, value = metric.Value.Percentile99 },
                                                   // new Quantile(){quantile = 0.999, value = metric.Value.Percentile999}
                                               }
                                           },
                                 label = metric.Tags.ToLabelPairs(labelNameFormatter)
                             }
                         };

            return result;
        }

        public static IEnumerable<Metric> ToPrometheusMetrics(
            this BucketHistogramValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            return BucketHistogramValueToHistogram(metric.Value, metric.Tags, labelNameFormatter);
        }

        private static IEnumerable<Metric> BucketHistogramValueToHistogram(
            BucketHistogramValue value,
            MetricTags tags,
            Func<string, string> labelNameFormatter)
        {
            var histogram = new Histogram
            {
                sample_count = (ulong)value.Count,
                sample_sum = value.Sum
            };

            var cumulativeCount = 0ul;
            foreach (var keyValuePair in value.Buckets.OrderBy(x => x.Key))
            {
                histogram.bucket.Add(new Bucket
                {
                    cumulative_count = cumulativeCount += Convert.ToUInt64(keyValuePair.Value),
                    upper_bound = keyValuePair.Key
                });
            }

            var result = new List<Metric>
            {
                new Metric
                {
                    histogram = histogram,
                    label = tags.ToLabelPairs(labelNameFormatter)
                }
            };

            return result;
        }

        public static IEnumerable<Metric> ToPrometheusMetrics(
            this BucketTimerValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            return BucketHistogramValueToHistogram(metric.Value.Histogram, metric.Tags, labelNameFormatter);
        }

        public static IEnumerable<Metric> ToPrometheusMetrics(
            this TimerValueSource metric,
            Func<string, string> labelNameFormatter)
        {
            // Prometheus advocates always using seconds as a base unit for time
            var rescaledVal = metric.Value.Scale(TimeUnit.Seconds, TimeUnit.Seconds);
            var result = new List<Metric>
                         {
                             new Metric
                             {
                                 summary = new Summary
                                           {
                                               sample_count = (ulong)rescaledVal.Rate.Count,
                                               sample_sum = rescaledVal.Histogram.Sum,
                                               quantile =
                                               {
                                                   new Quantile { quantile = 0.5, value = rescaledVal.Histogram.Median },
                                                   new Quantile { quantile = 0.75, value = rescaledVal.Histogram.Percentile75 },
                                                   new Quantile { quantile = 0.95, value = rescaledVal.Histogram.Percentile95 },
                                                   // new Quantile(){quantile = 0.98, value = metric.Value.Histogram.Percentile98},
                                                   new Quantile { quantile = 0.99, value = rescaledVal.Histogram.Percentile99 },
                                                   // new Quantile(){quantile = 0.999, value = metric.Value.Histogram.Percentile999}
                                               }
                                           },
                                 label = metric.Tags.ToLabelPairs(labelNameFormatter)
                             }
                         };

            return result;
        }
    }
}