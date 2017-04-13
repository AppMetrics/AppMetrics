// <copyright file="MetricValueExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Core.Internal;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Reporting;

namespace App.Metrics.Core.Extensions
{
    internal static class MetricValueExtensions
    {
        public static void AddApdexValues(
            this ApdexValue apdex,
            IDictionary<string, object> values,
            IDictionary<ApdexValueDataKeys, string> customDataKeys = null)
        {
            var dataKeys = Constants.DataKeyMapping.Apdex.MergeDifference(customDataKeys);

            values.Add(dataKeys[ApdexValueDataKeys.Samples], apdex.SampleSize);
            values.AddIfNotNanOrInfinity(dataKeys[ApdexValueDataKeys.Score], apdex.Score);
            values.Add(dataKeys[ApdexValueDataKeys.Satisfied], apdex.Satisfied);
            values.Add(dataKeys[ApdexValueDataKeys.Tolerating], apdex.Tolerating);
            values.Add(dataKeys[ApdexValueDataKeys.Frustrating], apdex.Frustrating);
        }

        public static void AddHistogramValues(
            this HistogramValue histogram,
            IDictionary<string, object> values,
            IDictionary<HistogramDataKeys, string> customDataKeys = null)
        {
            var dataKeys = Constants.DataKeyMapping.Histogram.MergeDifference(customDataKeys);

            values.Add(dataKeys[HistogramDataKeys.Samples], histogram.SampleSize);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.LastValue], histogram.LastValue);
            values.Add(dataKeys[HistogramDataKeys.Count], histogram.Count);
            values.Add(dataKeys[HistogramDataKeys.Sum], histogram.Sum);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.Min], histogram.Min);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.Max], histogram.Max);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.Mean], histogram.Mean);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.Median], histogram.Median);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.StdDev], histogram.StdDev);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.P999], histogram.Percentile999);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.P99], histogram.Percentile99);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.P98], histogram.Percentile98);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.P95], histogram.Percentile95);
            values.AddIfNotNanOrInfinity(dataKeys[HistogramDataKeys.P75], histogram.Percentile75);
            values.AddIfPresent(dataKeys[HistogramDataKeys.UserLastValue], histogram.LastUserValue);
            values.AddIfPresent(dataKeys[HistogramDataKeys.UserMinValue], histogram.MinUserValue);
            values.AddIfPresent(dataKeys[HistogramDataKeys.UserMaxValue], histogram.MaxUserValue);
        }

        public static void AddMeterSetItemValues(
            this MeterValue.SetItem meterItem,
            IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> customDataKeys = null)
        {
            var dataKeys = Constants.DataKeyMapping.Meter.MergeDifference(customDataKeys);

            AddMeterKeyValues(meterItem.Value, values, dataKeys);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.SetItemPercent], meterItem.Percent);
        }

        public static void AddMeterValues(
            this MeterValue meter,
            IDictionary<string, object> values,
            IDictionary<MeterValueDataKeys, string> customDataKeys = null)
        {
            var dataKeys = Constants.DataKeyMapping.Meter.MergeDifference(customDataKeys);

            AddMeterKeyValues(meter, values, dataKeys);
        }

        private static void AddMeterKeyValues(MeterValue meter, IDictionary<string, object> values, IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            values.Add(dataKeys[MeterValueDataKeys.Count], meter.Count);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.Rate1M], meter.OneMinuteRate);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.Rate5M], meter.FiveMinuteRate);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.Rate15M], meter.FifteenMinuteRate);
            values.AddIfNotNanOrInfinity(dataKeys[MeterValueDataKeys.RateMean], meter.MeanRate);
        }
    }
}