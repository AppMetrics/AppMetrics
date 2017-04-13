// <copyright file="PackMetricValueSourceExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Extensions;
using App.Metrics.Core.Internal;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Tagging;
using App.Metrics.Timer;

namespace App.Metrics.Reporting
{
    public static class PackMetricValueSourceExtensions
    {
        public static void PackApdex<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<ApdexValue> valueSource,
            Dictionary<ApdexValueDataKeys, string> customDataKeys = null)
        {
            if (valueSource == null)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            valueSource.Value.AddApdexValues(data, customDataKeys);
            PackMetric(payloadBuilder, metricNameFormatter, context, valueSource, data);
        }

        public static void PackCounter<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<CounterValue> valueSource,
            CounterValueSource counterValueSource)
        {
            if (counterValueSource == null)
            {
                return;
            }

            if (counterValueSource.Value.Items.Any() && counterValueSource.ReportSetItems)
            {
                foreach (var item in counterValueSource.Value.Items.Distinct())
                {
                    var itemData = new Dictionary<string, object> { { Constants.Pack.ItemDataTotalKey, item.Count } };

                    if (counterValueSource.ReportItemPercentages)
                    {
                        itemData.AddIfNotNanOrInfinity(Constants.Pack.ItemDataPercentKey, item.Percent);
                    }

                    PackMetricWithSetItems(payloadBuilder, metricNameFormatter, context, valueSource, item.Tags, itemData);
                }
            }

            var count = valueSource.ValueProvider.GetValue(resetMetric: counterValueSource.ResetOnReporting).Count;
            PackMetricValue(payloadBuilder, metricNameFormatter, context, valueSource, count);
        }

        public static void PackGauge<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<double> valueSource)
        {
            if (!double.IsNaN(valueSource.Value) && !double.IsInfinity(valueSource.Value))
            {
                PackMetricValue(payloadBuilder, metricNameFormatter, context, valueSource, valueSource.Value);
            }
        }

        public static void PackHistogram<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<HistogramValue> valueSource,
            Dictionary<HistogramDataKeys, string> customDataKeys = null)
        {
            var data = new Dictionary<string, object>();
            valueSource.Value.AddHistogramValues(data, customDataKeys);
            PackMetric(payloadBuilder, metricNameFormatter, context, valueSource, data);
        }

        public static void PackMeter<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<MeterValue> valueSource,
            Dictionary<MeterValueDataKeys, string> customDataKeys = null)
        {
            if (valueSource.Value.Items.Any())
            {
                foreach (var item in valueSource.Value.Items.Distinct())
                {
                    var setItemData = new Dictionary<string, object>();
                    item.Value.AddMeterValues(setItemData, customDataKeys);
                    setItemData.AddIfNotNanOrInfinity(Constants.Pack.ItemDataPercentKey, item.Percent);
                    PackMetricWithSetItems(payloadBuilder, metricNameFormatter, context, valueSource, item.Tags, setItemData);
                }
            }

            var data = new Dictionary<string, object>();
            valueSource.Value.AddMeterValues(data, customDataKeys);

            PackMetric(payloadBuilder, metricNameFormatter, context, valueSource, data);
        }

        public static void PackTimer<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<TimerValue> valueSource,
            Dictionary<MeterValueDataKeys, string> customMeterDataKeys = null,
            Dictionary<HistogramDataKeys, string> customHistogramDataKeys = null)
        {
            var data = new Dictionary<string, object>();
            valueSource.Value.Rate.AddMeterValues(data, customMeterDataKeys);
            valueSource.Value.Histogram.AddHistogramValues(data, customHistogramDataKeys);

            PackMetric(payloadBuilder, metricNameFormatter, context, valueSource, data);
        }

        private static MetricTags ConcatMetricTags<T>(MetricValueSourceBase<T> valueSource, MetricTags setItemTags)
        {
            var tagsWithSetItems = MetricTags.Concat(valueSource.Tags, setItemTags);
            var tags = Constants.Pack.MetricValueSourceTypeMapping.ContainsKey(typeof(T))
                ? MetricTags.Concat(tagsWithSetItems, new MetricTags(Constants.Pack.MetricTagsTypeKey, Constants.Pack.MetricValueSourceTypeMapping[typeof(T)]))
                : tagsWithSetItems;

            return tags;
        }

        private static MetricTags ConcatMetricTags<T>(MetricValueSourceBase<T> valueSource)
        {
            var tags = Constants.Pack.MetricValueSourceTypeMapping.ContainsKey(typeof(T))
                ? MetricTags.Concat(valueSource.Tags, new MetricTags(Constants.Pack.MetricTagsTypeKey, Constants.Pack.MetricValueSourceTypeMapping[typeof(T)]))
                : valueSource.Tags;
            return tags;
        }

        private static void PackMetric<T1, T2>(
            IMetricPayloadBuilder<T1> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<T2> valueSource,
            Dictionary<string, object> data)
        {
            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);
            var tags = ConcatMetricTags(valueSource);

            if (valueSource.IsMultidimensional)
            {
                payloadBuilder.Pack(
                    metricNameFormatter(context, valueSource.MultidimensionalName),
                    keys,
                    values,
                    tags);

                return;
            }

            payloadBuilder.Pack(metricNameFormatter(context, valueSource.Name), keys, values, tags);
        }

        private static void PackMetricValue<T, TU>(
            IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<TU> valueSource,
            object value)
        {
            var tags = ConcatMetricTags(valueSource);

            if (valueSource.IsMultidimensional)
            {
                payloadBuilder.Pack(
                    metricNameFormatter(context, valueSource.MultidimensionalName),
                    value,
                    tags);

                return;
            }

            payloadBuilder.Pack(metricNameFormatter(context, valueSource.Name), value, tags);
        }

        private static void PackMetricWithSetItems<T, TU>(
            IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<TU> valueSource,
            MetricTags setItemTags,
            Dictionary<string, object> itemData)
        {
            var keys = itemData.Keys.ToList();
            var values = keys.Select(k => itemData[k]);
            var tags = ConcatMetricTags(valueSource, setItemTags);

            if (valueSource.IsMultidimensional)
            {
                payloadBuilder.Pack(
                    metricNameFormatter(context, valueSource.MultidimensionalName + Constants.Pack.MetricSetItemSuffix),
                    keys,
                    values,
                    tags);

                return;
            }

            payloadBuilder.Pack(metricNameFormatter(context, valueSource.Name + Constants.Pack.MetricSetItemSuffix), keys, values, tags);
        }
    }
}