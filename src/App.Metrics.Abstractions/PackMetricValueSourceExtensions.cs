// <copyright file="PackMetricValueSourceExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics
{
    public static class PackMetricValueSourceExtensions
    {
        public static void PackApdex<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            string context,
            MetricValueSourceBase<ApdexValue> valueSource)
        {
            if (valueSource == null)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            valueSource.Value.AddApdexValues(data, payloadBuilder.DataKeys.Apdex);
            PackMetric(payloadBuilder, context, valueSource, data);
        }

        public static void PackCounter<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
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
                    var itemData = new Dictionary<string, object> { { payloadBuilder.DataKeys.Counter[CounterValueDataKeys.Total], item.Count } };

                    if (counterValueSource.ReportItemPercentages)
                    {
                        itemData.AddIfNotNanOrInfinity(payloadBuilder.DataKeys.Counter[CounterValueDataKeys.SetItemPercent], item.Percent);
                    }

                    PackMetricWithSetItems(
                        payloadBuilder,
                        context,
                        valueSource,
                        item.Tags,
                        itemData,
                        payloadBuilder.DataKeys.Counter[CounterValueDataKeys.MetricSetItemSuffix]);
                }
            }

            var count = valueSource.ValueProvider.GetValue(resetMetric: counterValueSource.ResetOnReporting).Count;
            PackMetricValue(payloadBuilder, context, valueSource, count);
        }

        public static void PackGauge<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            string context,
            MetricValueSourceBase<double> valueSource)
        {
            if (!double.IsNaN(valueSource.Value) && !double.IsInfinity(valueSource.Value))
            {
                PackMetricValue(payloadBuilder, context, valueSource, valueSource.Value);
            }
        }

        public static void PackHistogram<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            string context,
            MetricValueSourceBase<HistogramValue> valueSource)
        {
            var data = new Dictionary<string, object>();
            valueSource.Value.AddHistogramValues(data, payloadBuilder.DataKeys.Histogram);
            PackMetric(payloadBuilder, context, valueSource, data);
        }

        public static void PackMeter<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            string context,
            MetricValueSourceBase<MeterValue> valueSource)
        {
            if (valueSource.Value.Items.Any())
            {
                foreach (var item in valueSource.Value.Items.Distinct())
                {
                    item.AddMeterSetItemValues(out IDictionary<string, object> setItemData, payloadBuilder.DataKeys.Meter);
                    PackMetricWithSetItems(
                        payloadBuilder,
                        context,
                        valueSource,
                        item.Tags,
                        setItemData,
                        payloadBuilder.DataKeys.Meter[MeterValueDataKeys.MetricSetItemSuffix]);
                }
            }

            valueSource.Value.AddMeterValues(out IDictionary<string, object> data, payloadBuilder.DataKeys.Meter);

            PackMetric(payloadBuilder, context, valueSource, data);
        }

        public static void PackTimer<T>(
            this IMetricPayloadBuilder<T> payloadBuilder,
            string context,
            MetricValueSourceBase<TimerValue> valueSource)
        {
            valueSource.Value.Rate.AddMeterValues(out IDictionary<string, object> data, payloadBuilder.DataKeys.Meter);
            valueSource.Value.Histogram.AddHistogramValues(data, payloadBuilder.DataKeys.Histogram);

            PackMetric(payloadBuilder, context, valueSource, data);
        }

        private static MetricTags ConcatIntrinsicMetricTags<T>(MetricValueSourceBase<T> valueSource)
        {
            var intrinsicTags = new MetricTags(AppMetricsConstants.Pack.MetricTagsUnitKey, valueSource.Unit.ToString());

            if (typeof(T) == typeof(TimerValue))
            {
                var timerValueSource = valueSource as MetricValueSourceBase<TimerValue>;

                var timerIntrinsicTags = new MetricTags(
                    new[] { AppMetricsConstants.Pack.MetricTagsUnitRateDurationKey, AppMetricsConstants.Pack.MetricTagsUnitRateKey },
                    new[] { timerValueSource.Value.DurationUnit.Unit(), timerValueSource.Value.Rate.RateUnit.Unit() });

                intrinsicTags = MetricTags.Concat(intrinsicTags, timerIntrinsicTags);
            }
            else if (typeof(T) == typeof(MeterValue))
            {
                var meterValueSource = valueSource as MetricValueSourceBase<MeterValue>;

                var meterIntrinsicTags = new MetricTags(AppMetricsConstants.Pack.MetricTagsUnitRateKey, meterValueSource.Value.RateUnit.Unit());
                intrinsicTags = MetricTags.Concat(intrinsicTags, meterIntrinsicTags);
            }

            if (AppMetricsConstants.Pack.MetricValueSourceTypeMapping.ContainsKey(typeof(T)))
            {
                var metricTypeTag = new MetricTags(AppMetricsConstants.Pack.MetricTagsTypeKey, AppMetricsConstants.Pack.MetricValueSourceTypeMapping[typeof(T)]);
                intrinsicTags = MetricTags.Concat(metricTypeTag, intrinsicTags);
            }
            else
            {
                var metricTypeTag = new MetricTags(AppMetricsConstants.Pack.MetricTagsTypeKey, typeof(T).Name.ToLowerInvariant());
                intrinsicTags = MetricTags.Concat(metricTypeTag, intrinsicTags);
            }

            return intrinsicTags;
        }

        private static MetricTags ConcatMetricTags<T>(MetricValueSourceBase<T> valueSource, MetricTags setItemTags)
        {
            var tagsWithSetItems = MetricTags.Concat(valueSource.Tags, setItemTags);
            var intrinsicTags = ConcatIntrinsicMetricTags(valueSource);

            return MetricTags.Concat(tagsWithSetItems, intrinsicTags);
        }

        private static MetricTags ConcatMetricTags<T>(MetricValueSourceBase<T> valueSource)
        {
            var intrinsicTags = ConcatIntrinsicMetricTags(valueSource);

            return MetricTags.Concat(valueSource.Tags, intrinsicTags);
        }

        private static void PackMetric<T1, T2>(
            IMetricPayloadBuilder<T1> payloadBuilder,
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
                    context,
                    valueSource.MultidimensionalName,
                    keys,
                    values,
                    tags);

                return;
            }

            payloadBuilder.Pack(context, valueSource.Name, keys, values, tags);
        }

        private static void PackMetricValue<T, TU>(
            IMetricPayloadBuilder<T> payloadBuilder,
            string context,
            MetricValueSourceBase<TU> valueSource,
            object value)
        {
            var tags = ConcatMetricTags(valueSource);

            if (valueSource.IsMultidimensional)
            {
                payloadBuilder.Pack(
                    context,
                    valueSource.MultidimensionalName,
                    value,
                    tags);

                return;
            }

            payloadBuilder.Pack(context, valueSource.Name, value, tags);
        }

        private static void PackMetricWithSetItems<T, TU>(
            IMetricPayloadBuilder<T> payloadBuilder,
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
                    context,
                    valueSource.MultidimensionalName + metricSetItemSuffix,
                    keys,
                    values,
                    tags);

                return;
            }

            payloadBuilder.Pack(context, valueSource.Name + metricSetItemSuffix, keys, values, tags);
        }
    }
}