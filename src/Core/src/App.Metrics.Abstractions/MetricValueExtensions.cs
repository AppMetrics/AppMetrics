// <copyright file="MetricValueExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace App.Metrics
{
    public static class MetricValueExtensions
    {
        public static void AddApdexValues(
            this ApdexValue apdex,
            IDictionary<string, object> values,
            IDictionary<ApdexFields, string> fields)
        {
            if (values == null)
            {
                return;
            }

            fields.TryAddValuesForKey(values, ApdexFields.Samples, apdex.SampleSize);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, ApdexFields.Score, apdex.Score);
            fields.TryAddValuesForKey(values, ApdexFields.Satisfied, apdex.Satisfied);
            fields.TryAddValuesForKey(values, ApdexFields.Tolerating, apdex.Tolerating);
            fields.TryAddValuesForKey(values, ApdexFields.Frustrating, apdex.Frustrating);
        }

        public static void AddHistogramValues(
            this HistogramValue histogram,
            IDictionary<string, object> values,
            IDictionary<HistogramFields, string> fields)
        {
            if (values == null)
            {
                return;
            }

            fields.TryAddValuesForKey(values, HistogramFields.Samples, histogram.SampleSize);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.LastValue, histogram.LastValue);
            fields.TryAddValuesForKey(values, HistogramFields.Count, histogram.Count);
            fields.TryAddValuesForKey(values, HistogramFields.Sum, histogram.Sum);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Min, histogram.Min);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Max, histogram.Max);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Mean, histogram.Mean);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Median, histogram.Median);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.StdDev, histogram.StdDev);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P999, histogram.Percentile999);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P99, histogram.Percentile99);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P98, histogram.Percentile98);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P95, histogram.Percentile95);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P75, histogram.Percentile75);
            fields.TryAddValuesForKeyIfPresent(values, HistogramFields.UserLastValue, histogram.LastUserValue);
            fields.TryAddValuesForKeyIfPresent(values, HistogramFields.UserMinValue, histogram.MinUserValue);
            fields.TryAddValuesForKeyIfPresent(values, HistogramFields.UserMaxValue, histogram.MaxUserValue);
        }

        public static void AddBucketHistogramValues(
            this BucketHistogramValue histogram,
            IDictionary<string, object> values,
            IDictionary<string, string> fields)
        {
            if (values == null)
            {
                return;
            }

            fields.TryAddValuesForKey(values, BucketHistogramFields.Count.ToString(), histogram.Count);
            fields.TryAddValuesForKey(values, BucketHistogramFields.Sum.ToString(), histogram.Sum);
            foreach (var bucket in histogram.Buckets)
            {
                if (double.IsPositiveInfinity(bucket.Key))
                {
                    values[$"{BucketHistogramFields.Bucket}Inf"] = bucket.Value;
                }
                else
                {
                    values[$"{BucketHistogramFields.Bucket}{bucket.Key}"] = bucket.Value;
                }
            }
        }

        public static void AddBucketTimerValues(
            this BucketTimerValue timer,
            IDictionary<string, object> values,
            IDictionary<string, string> fields)
        {
            if (values == null)
            {
                return;
            }

            fields.TryAddValuesForKey(values, BucketHistogramFields.Count.ToString(), timer.Histogram.Count);
            fields.TryAddValuesForKey(values, BucketHistogramFields.Sum.ToString(), timer.Histogram.Sum);
            foreach (var bucket in timer.Histogram.Buckets)
            {
                if (double.IsPositiveInfinity(bucket.Key))
                {
                    values[$"{BucketHistogramFields.Bucket}Inf"] = bucket.Value;
                }
                else
                {
                    values[$"{BucketHistogramFields.Bucket}{bucket.Key}"] = bucket.Value;
                }
            }
        }

        public static void AddMeterSetItemValues(
            this MeterValue.SetItem meterSetItem,
            IDictionary<string, object> values,
            IDictionary<MeterFields, string> fields)
        {
            if (values == null || !fields.ContainsKey(MeterFields.SetItem))
            {
                return;
            }

            AddMeterValues(meterSetItem.Value, values, fields);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.SetItemPercent, meterSetItem.Percent);
        }

        public static void AddMeterValues(
            this MeterValue meter,
            IDictionary<string, object> values,
            IDictionary<MeterFields, string> fields)
        {
            if (values == null)
            {
                return;
            }

            fields.TryAddValuesForKey(values, MeterFields.Count, meter.Count);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.Rate1M, meter.OneMinuteRate);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.Rate5M, meter.FiveMinuteRate);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.Rate15M, meter.FifteenMinuteRate);
            fields.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.RateMean, meter.MeanRate);
        }
    }
}