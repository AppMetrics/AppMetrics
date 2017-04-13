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
            IDictionary<ApdexValueDataKeys, string> dataKeys)
        {
            if (valueSource == null)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            valueSource.Value.AddApdexValues(data, dataKeys);
            PackMetric(payloadBuilder, metricNameFormatter, context, valueSource, data);
        }

        public static void PackCounter<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<CounterValue> valueSource,
            CounterValueSource counterValueSource,
            IDictionary<CounterValueDataKeys, string> dataKeys)
        {
            if (counterValueSource == null)
            {
                return;
            }

            if (counterValueSource.Value.Items.Any() && counterValueSource.ReportSetItems)
            {
                foreach (var item in counterValueSource.Value.Items.Distinct())
                {
                    var itemData = new Dictionary<string, object> { { dataKeys[CounterValueDataKeys.Total], item.Count } };

                    if (counterValueSource.ReportItemPercentages)
                    {
                        itemData.AddIfNotNanOrInfinity(dataKeys[CounterValueDataKeys.SetItemPercent], item.Percent);
                    }

                    PackMetricWithSetItems(payloadBuilder, metricNameFormatter, context, valueSource, item.Tags, itemData, dataKeys[CounterValueDataKeys.MetricSetItemSuffix]);
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
            IDictionary<HistogramValueDataKeys, string> dataKeys)
        {
            var data = new Dictionary<string, object>();
            valueSource.Value.AddHistogramValues(data, dataKeys);
            PackMetric(payloadBuilder, metricNameFormatter, context, valueSource, data);
        }

        public static void PackMeter<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<MeterValue> valueSource,
            IDictionary<MeterValueDataKeys, string> dataKeys)
        {
            if (valueSource.Value.Items.Any())
            {
                foreach (var item in valueSource.Value.Items.Distinct())
                {
                    item.AddMeterSetItemValues(out IDictionary<string, object> setItemData, dataKeys);
                    PackMetricWithSetItems(payloadBuilder, metricNameFormatter, context, valueSource, item.Tags, setItemData, dataKeys[MeterValueDataKeys.MetricSetItemSuffix]);
                }
            }

            valueSource.Value.AddMeterValues(out IDictionary<string, object> data, dataKeys);

            PackMetric(payloadBuilder, metricNameFormatter, context, valueSource, data);
        }

        public static void PackTimer<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            Func<string, string, string> metricNameFormatter,
            string context,
            MetricValueSourceBase<TimerValue> valueSource,
            IDictionary<MeterValueDataKeys, string> meterDataKeys,
            IDictionary<HistogramValueDataKeys, string> histogramDataKeys)
        {
            valueSource.Value.Rate.AddMeterValues(out IDictionary<string, object> data, meterDataKeys);
            valueSource.Value.Histogram.AddHistogramValues(data, histogramDataKeys);

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
            IDictionary<string, object> data)
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
            IDictionary<string, object> itemData,
            string metricSetItemSuffix)
        {
            var keys = itemData.Keys.ToList();
            var values = keys.Select(k => itemData[k]);
            var tags = ConcatMetricTags(valueSource, setItemTags);

            if (valueSource.IsMultidimensional)
            {
                payloadBuilder.Pack(
                    metricNameFormatter(context, valueSource.MultidimensionalName + metricSetItemSuffix),
                    keys,
                    values,
                    tags);

                return;
            }

            payloadBuilder.Pack(metricNameFormatter(context, valueSource.Name + metricSetItemSuffix), keys, values, tags);
        }
    }
}