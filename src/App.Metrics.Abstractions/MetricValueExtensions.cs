// <copyright file="MetricValueExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
            IDictionary<ApdexValueDataKeys, string> dataKeys)
        {
            if (values == null)
            {
                return;
            }

            dataKeys.TryAddValuesForKey(values, ApdexValueDataKeys.Samples, apdex.SampleSize);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, ApdexValueDataKeys.Score, apdex.Score);
            dataKeys.TryAddValuesForKey(values, ApdexValueDataKeys.Satisfied, apdex.Satisfied);
            dataKeys.TryAddValuesForKey(values, ApdexValueDataKeys.Tolerating, apdex.Tolerating);
            dataKeys.TryAddValuesForKey(values, ApdexValueDataKeys.Frustrating, apdex.Frustrating);
        }

        public static void AddHistogramValues(
            this HistogramValue histogram,
            IDictionary<string, object> values,
            IDictionary<HistogramValueDataKeys, string> dataKeys)
        {
            if (values == null)
            {
                return;
            }

            dataKeys.TryAddValuesForKey(values, HistogramValueDataKeys.Samples, histogram.SampleSize);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.LastValue, histogram.LastValue);
            dataKeys.TryAddValuesForKey(values, HistogramValueDataKeys.Count, histogram.Count);
            dataKeys.TryAddValuesForKey(values, HistogramValueDataKeys.Sum, histogram.Sum);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.Min, histogram.Min);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.Max, histogram.Max);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.Mean, histogram.Mean);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.Median, histogram.Median);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.StdDev, histogram.StdDev);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.P999, histogram.Percentile999);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.P99, histogram.Percentile99);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.P98, histogram.Percentile98);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.P95, histogram.Percentile95);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, HistogramValueDataKeys.P75, histogram.Percentile75);
            dataKeys.TryAddValuesForKeyIfPresent(values, HistogramValueDataKeys.UserLastValue, histogram.LastUserValue);
            dataKeys.TryAddValuesForKeyIfPresent(values, HistogramValueDataKeys.UserMinValue, histogram.MinUserValue);
            dataKeys.TryAddValuesForKeyIfPresent(values, HistogramValueDataKeys.UserMaxValue, histogram.MaxUserValue);
        }

        public static void AddMeterSetItemValues(
            this MeterValue.SetItem meterItem,
            IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            if (values == null)
            {
                return;
            }

            AddMeterKeyValues(meterItem.Value, values, dataKeys);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterValueDataKeys.SetItemPercent, meterItem.Percent);
        }

        public static void AddMeterValues(
            this MeterValue meter,
            IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            if (values == null)
            {
                return;
            }

            AddMeterKeyValues(meter, values, dataKeys);
        }

        private static void AddMeterKeyValues(
            MeterValue meter,
            IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            if (values == null)
            {
                return;
            }

            dataKeys.TryAddValuesForKey(values, MeterValueDataKeys.Count, meter.Count);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterValueDataKeys.Rate1M, meter.OneMinuteRate);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterValueDataKeys.Rate5M, meter.FiveMinuteRate);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterValueDataKeys.Rate15M, meter.FifteenMinuteRate);
            dataKeys.TryAddValuesForKeyIfNotNanOrInfinity(values, MeterValueDataKeys.RateMean, meter.MeanRate);
        }
    }
}