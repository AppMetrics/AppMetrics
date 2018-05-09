// <copyright file="MetricValueExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace App.Metrics
{
    public static class MetricValueExtensions
    {
        public static void AddApdexValues(
            this ApdexValue apdex,
            IDictionary<string, object> values,
            IDictionary<ApdexFields, string> fieldMapping)
        {
            if (values == null)
            {
                return;
            }

            fieldMapping.TryAddValuesForKey(values, ApdexFields.Samples, apdex.SampleSize);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, ApdexFields.Score, apdex.Score);
            fieldMapping.TryAddValuesForKey(values, ApdexFields.Satisfied, apdex.Satisfied);
            fieldMapping.TryAddValuesForKey(values, ApdexFields.Tolerating, apdex.Tolerating);
            fieldMapping.TryAddValuesForKey(values, ApdexFields.Frustrating, apdex.Frustrating);
        }

        public static void AddHistogramValues(
            this HistogramValue histogram,
            IDictionary<string, object> values,
            IDictionary<HistogramFields, string> fieldMapping)
        {
            if (values == null)
            {
                return;
            }

            fieldMapping.TryAddValuesForKey(values, HistogramFields.Samples, histogram.SampleSize);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.LastValue, histogram.LastValue);
            fieldMapping.TryAddValuesForKey(values, HistogramFields.Count, histogram.Count);
            fieldMapping.TryAddValuesForKey(values, HistogramFields.Sum, histogram.Sum);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Min, histogram.Min);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Max, histogram.Max);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Mean, histogram.Mean);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.Median, histogram.Median);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.StdDev, histogram.StdDev);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P999, histogram.Percentile999);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P99, histogram.Percentile99);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P98, histogram.Percentile98);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P95, histogram.Percentile95);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramFields.P75, histogram.Percentile75);
            fieldMapping.TryAddValuesForKeyIfPresent(values, HistogramFields.UserLastValue, histogram.LastUserValue);
            fieldMapping.TryAddValuesForKeyIfPresent(values, HistogramFields.UserMinValue, histogram.MinUserValue);
            fieldMapping.TryAddValuesForKeyIfPresent(values, HistogramFields.UserMaxValue, histogram.MaxUserValue);
        }

        public static void AddMeterSetItemValues(
            this MeterValue.SetItem meterSetItem,
            IDictionary<string, object> values,
            IDictionary<MeterFields, string> meterFieldMapping)
        {
            if (values == null || !meterFieldMapping.ContainsKey(MeterFields.SetItem))
            {
                return;
            }

            AddMeterValues(meterSetItem.Value, values, meterFieldMapping);
            meterFieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.SetItemPercent, meterSetItem.Percent);
        }

        public static void AddMeterValues(
            this MeterValue meter,
            IDictionary<string, object> values,
            IDictionary<MeterFields, string> fieldMapping)
        {
            if (values == null)
            {
                return;
            }

            fieldMapping.TryAddValuesForKey(values, MeterFields.Count, meter.Count);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.Rate1M, meter.OneMinuteRate);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.Rate5M, meter.FiveMinuteRate);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.Rate15M, meter.FifteenMinuteRate);
            fieldMapping.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterFields.RateMean, meter.MeanRate);
        }
    }
}