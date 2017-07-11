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
            values.Add(dataKeys[ApdexValueDataKeys.Samples], apdex.SampleSize);
            values.AddIfNotNanOrInfinity(dataKeys[ApdexValueDataKeys.Score], apdex.Score);
            values.Add(dataKeys[ApdexValueDataKeys.Satisfied], apdex.Satisfied);
            values.Add(dataKeys[ApdexValueDataKeys.Tolerating], apdex.Tolerating);
            values.Add(dataKeys[ApdexValueDataKeys.Frustrating], apdex.Frustrating);
        }

        public static void AddHistogramValues(
            this HistogramValue histogram,
            IDictionary<string, object> values,
            IDictionary<HistogramValueDataKeys, string> dataKeys)
        {
            values.Add(dataKeys[HistogramValueDataKeys.Samples], histogram.SampleSize);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.LastValue], histogram.LastValue);
            values.Add(dataKeys[HistogramValueDataKeys.Count], histogram.Count);
            values.Add(dataKeys[HistogramValueDataKeys.Sum], histogram.Sum);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.Min], histogram.Min);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.Max], histogram.Max);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.Mean], histogram.Mean);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.Median], histogram.Median);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.StdDev], histogram.StdDev);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.P999], histogram.Percentile999);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.P99], histogram.Percentile99);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.P98], histogram.Percentile98);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.P95], histogram.Percentile95);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramValueDataKeys.P75], histogram.Percentile75);
            values.AddIfPresent(dataKeys[HistogramValueDataKeys.UserLastValue], histogram.LastUserValue);
            values.AddIfPresent(dataKeys[HistogramValueDataKeys.UserMinValue], histogram.MinUserValue);
            values.AddIfPresent(dataKeys[HistogramValueDataKeys.UserMaxValue], histogram.MaxUserValue);
        }

        public static void AddMeterSetItemValues(
            this MeterValue.SetItem meterItem,
            out IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            AddMeterKeyValues(meterItem.Value, out values, dataKeys);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.SetItemPercent], meterItem.Percent);
        }

        public static void AddMeterValues(
            this MeterValue meter,
            out IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            AddMeterKeyValues(meter, out values, dataKeys);
        }

        private static void AddMeterKeyValues(
            MeterValue meter,
            out IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            // ReSharper disable UseObjectOrCollectionInitializer
            values = new Dictionary<string, object>();
            // ReSharper restore UseObjectOrCollectionInitializer

            values.Add(dataKeys[MeterValueDataKeys.Count], meter.Count);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.Rate1M], meter.OneMinuteRate);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.Rate5M], meter.FiveMinuteRate);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.Rate15M], meter.FifteenMinuteRate);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.RateMean], meter.MeanRate);
        }
    }
}