// <copyright file="MetricSnapshotWriterExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Serialization
{
    public static class MetricSnapshotWriterExtensions
    {
        public static void WriteApdex(
            this IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<ApdexValue> valueSource,
            IDictionary<ApdexFields, string> fields,
            DateTime timestamp)
        {
            if (valueSource == null || fields.Count == 0)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            var value = valueSource.ValueProvider.GetValue(valueSource.ResetOnReporting);
            value.AddApdexValues(data, fields);
            WriteMetric(writer, context, valueSource, data, timestamp);
        }

        public static void WriteCounter(
            this IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<CounterValue> valueSource,
            CounterValueSource counterValueSource,
            IDictionary<CounterFields, string> fields,
            DateTime timestamp)
        {
            if (counterValueSource == null || fields.Count == 0)
            {
                return;
            }

            var value = counterValueSource.ValueProvider.GetValue(counterValueSource.ResetOnReporting);

            if (value.Items.Any() && counterValueSource.ReportSetItems && fields.ContainsKey(CounterFields.SetItem))
            {
                var itemSuffix = fields[CounterFields.SetItem];

                foreach (var item in value.Items.Distinct())
                {
                    var itemData = new Dictionary<string, object>();

                    if (fields.ContainsKey(CounterFields.Total))
                    {
                        itemData.Add(fields[CounterFields.Total], item.Count);
                    }

                    if (counterValueSource.ReportItemPercentages && fields.ContainsKey(CounterFields.SetItemPercent))
                    {
                        itemData.AddIfNotNanOrInfinity(fields[CounterFields.SetItemPercent], item.Percent);
                    }

                    if (itemData.Any())
                    {
                        WriteMetricWithSetItems(
                            writer,
                            context,
                            valueSource,
                            item.Tags,
                            itemData,
                            itemSuffix,
                            timestamp);
                    }
                }
            }

            if (fields.ContainsKey(CounterFields.Value))
            {
                var count = value.Count;
                WriteMetricValue(writer, context, valueSource, fields[CounterFields.Value], count, timestamp);
            }
        }

        public static void WriteGauge(
            this IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<double> valueSource,
            IDictionary<GaugeFields, string> fields,
            DateTime timestamp)
        {
            if (valueSource == null || fields.Count == 0)
            {
                return;
            }

            var value = valueSource.ValueProvider.GetValue(valueSource.ResetOnReporting);

            if (!double.IsNaN(value) && !double.IsInfinity(value) && fields.ContainsKey(GaugeFields.Value))
            {
                WriteMetricValue(writer, context, valueSource, fields[GaugeFields.Value], value, timestamp);
            }
        }

        public static void WriteHistogram(
            this IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<HistogramValue> valueSource,
            IDictionary<HistogramFields, string> fields,
            DateTime timestamp)
        {
            if (valueSource == null || fields.Count == 0)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            var value = valueSource.ValueProvider.GetValue(valueSource.ResetOnReporting);
            value.AddHistogramValues(data, fields);
            WriteMetric(writer, context, valueSource, data, timestamp);
        }

        public static void WriteBucketHistogram(
            this IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<BucketHistogramValue> valueSource,
            IDictionary<string, string> fields,
            DateTime timestamp)
        {
            if (valueSource == null || fields.Count == 0)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            valueSource.Value.AddBucketHistogramValues(data, fields);
            WriteMetric(writer, context, valueSource, data, timestamp);
        }

        public static void WriteBucketTimer(
            this IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<BucketTimerValue> valueSource,
            IDictionary<string, string> fields,
            DateTime timestamp)
        {
            if (valueSource == null || fields.Count == 0)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            valueSource.Value.AddBucketTimerValues(data, fields);
            WriteMetric(writer, context, valueSource, data, timestamp);
        }

        public static void WriteMeter(
            this IMetricSnapshotWriter writer,
            string context,
            MeterValueSource valueSource,
            IDictionary<MeterFields, string> fields,
            DateTime timestamp)
        {
            if (valueSource == null || fields.Count == 0)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            var value = valueSource.ValueProvider.GetValue(valueSource.ResetOnReporting);

            if (value.Items.Any() && valueSource.ReportSetItems && fields.ContainsKey(MeterFields.SetItem))
            {
                var itemSuffix = fields[MeterFields.SetItem];

                foreach (var item in value.Items.Distinct())
                {
                    var setItemData = new Dictionary<string, object>();

                    item.AddMeterSetItemValues(setItemData, fields);

                    if (setItemData.Any())
                    {
                        WriteMetricWithSetItems(
                            writer,
                            context,
                            valueSource,
                            item.Tags,
                            setItemData,
                            itemSuffix,
                            timestamp);
                    }
                }
            }

            value.AddMeterValues(data, fields);

            WriteMetric(writer, context, valueSource, data, timestamp);
        }

        public static void WriteTimer(
            this IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<TimerValue> valueSource,
            IDictionary<MeterFields, string> meterFields,
            IDictionary<HistogramFields, string> histogramFields,
            DateTime timestamp)
        {
            if (valueSource == null)
            {
                return;
            }

            var data = new Dictionary<string, object>();
            var value = valueSource.ValueProvider.GetValue(valueSource.ResetOnReporting);

            if (meterFields.Count > 0)
            {
                value.Rate.AddMeterValues(data, meterFields);
            }

            if (histogramFields.Count > 0)
            {
                value.Histogram.AddHistogramValues(data, histogramFields);
            }

            if (data.Count > 0)
            {
                WriteMetric(writer, context, valueSource, data, timestamp);
            }
        }

        private static MetricTags ConcatIntrinsicMetricTags<T>(MetricValueSourceBase<T> valueSource)
        {
            var intrinsicTags = new MetricTags(AppMetricsConstants.Pack.MetricTagsUnitKey, valueSource.Unit.ToString());

            if (typeof(T) == typeof(TimerValue))
            {
                var timerValueSource = valueSource as MetricValueSourceBase<TimerValue>;

                var timerIntrinsicTags = new MetricTags(
                    new[] { AppMetricsConstants.Pack.MetricTagsUnitRateDurationKey, AppMetricsConstants.Pack.MetricTagsUnitRateKey },
                    new[] { timerValueSource?.Value.DurationUnit.Unit(), timerValueSource?.Value.Rate.RateUnit.Unit() });

                intrinsicTags = MetricTags.Concat(intrinsicTags, timerIntrinsicTags);
            }
            else if (typeof(T) == typeof(MeterValue))
            {
                var meterValueSource = valueSource as MetricValueSourceBase<MeterValue>;

                var meterIntrinsicTags = new MetricTags(AppMetricsConstants.Pack.MetricTagsUnitRateKey, meterValueSource?.Value.RateUnit.Unit());
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

        private static void WriteMetric<T>(
            IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<T> valueSource,
            IDictionary<string, object> data,
            DateTime timestamp)
        {
            var keys = data.Keys.ToList();
            var values = keys.Select(k => data[k]);
            var tags = ConcatMetricTags(valueSource);

            if (valueSource.IsMultidimensional)
            {
                writer.Write(
                    context,
                    valueSource.MultidimensionalName,
                    keys,
                    values,
                    tags,
                    timestamp);

                return;
            }

            writer.Write(context, valueSource.Name, keys, values, tags, timestamp);
        }

        private static void WriteMetricValue<T>(
            IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<T> valueSource,
            string field,
            object value,
            DateTime timestamp)
        {
            var tags = ConcatMetricTags(valueSource);

            if (valueSource.IsMultidimensional)
            {
                writer.Write(
                    context,
                    valueSource.MultidimensionalName,
                    field,
                    value,
                    tags,
                    timestamp);

                return;
            }

            writer.Write(context, valueSource.Name, field, value, tags, timestamp);
        }

        private static void WriteMetricWithSetItems<T>(
            IMetricSnapshotWriter writer,
            string context,
            MetricValueSourceBase<T> valueSource,
            MetricTags setItemTags,
            IDictionary<string, object> itemData,
            string metricSetItemSuffix,
            DateTime timestamp)
        {
            var keys = itemData.Keys.ToList();
            var values = keys.Select(k => itemData[k]);
            var tags = ConcatMetricTags(valueSource, setItemTags);

            if (valueSource.IsMultidimensional)
            {
                writer.Write(
                    context,
                    valueSource.MultidimensionalName + metricSetItemSuffix,
                    keys,
                    values,
                    tags,
                    timestamp);

                return;
            }

            writer.Write(context, valueSource.Name + metricSetItemSuffix, keys, values, tags, timestamp);
        }
    }
}
