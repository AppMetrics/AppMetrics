// <copyright file="MetricSnapshotSerializer.cs" company="App Metrics Contributors">
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
    /// <summary>
    ///     Serializes <see cref="MetricsDataValueSource" /> into the different formats.
    /// </summary>
    public class MetricSnapshotSerializer
    {
        /// <summary>
        ///     Serializes the specified <see cref="MetricsDataValueSource" /> and writes the metrics snapshot using the specified
        ///     <see cref="IMetricSnapshotWriter" />.
        /// </summary>
        /// <param name="writer">The <see cref="IMetricSnapshotWriter" /> used to write the metrics snapshot.</param>
        /// <param name="metricsData">The <see cref="MetricsDataValueSource" /> to serilize.</param>
        /// <param name="fields">The metric fields to write</param>
        public void Serialize(IMetricSnapshotWriter writer, MetricsDataValueSource metricsData, MetricFields fields)
        {
            foreach (var contextValueSource in metricsData.Contexts)
            {
                foreach (var valueSource in contextValueSource.ApdexScores)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }

                foreach (var valueSource in contextValueSource.Gauges)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }

                foreach (var valueSource in contextValueSource.Counters)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }

                foreach (var valueSource in contextValueSource.Meters)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }

                foreach (var valueSource in contextValueSource.Timers)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }

                foreach (var valueSource in contextValueSource.BucketTimers)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }

                foreach (var valueSource in contextValueSource.Histograms)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }

                foreach (var valueSource in contextValueSource.BucketHistograms)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer, fields, metricsData.Timestamp);
                }
            }
        }

        private static void BuildApdexPayload(
            string context,
            MetricValueSourceBase<ApdexValue> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<ApdexFields, string> fields,
            DateTime timestamp)
        {
            writer.WriteApdex(context, valueSource, fields, timestamp);
        }

        private static void BuildCounterPayload(
            string context,
            MetricValueSourceBase<CounterValue> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<CounterFields, string> fields,
            DateTime timestamp)
        {
            var counterValueSource = valueSource as CounterValueSource;
            writer.WriteCounter(context, valueSource, counterValueSource, fields, timestamp);
        }

        private static void BuildGaugePayload(
            string context,
            MetricValueSourceBase<double> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<GaugeFields, string> fields,
            DateTime timestamp)
        {
            writer.WriteGauge(context, valueSource, fields, timestamp);
        }

        private static void BuildHistogramPayload(
            string context,
            MetricValueSourceBase<HistogramValue> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<HistogramFields, string> fields,
            DateTime timestamp)
        {
            writer.WriteHistogram(context, valueSource, fields, timestamp);
        }

        private static void BuildBucketHistogramPayload(
            string context,
            MetricValueSourceBase<BucketHistogramValue> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<string, string> fields,
            DateTime timestamp)
        {
            writer.WriteBucketHistogram(context, valueSource, fields, timestamp);
        }

        private static void BuildBucketTimerPayload(
            string context,
            MetricValueSourceBase<BucketTimerValue> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<string, string> fields,
            DateTime timestamp)
        {
            writer.WriteBucketTimer(context, valueSource, fields, timestamp);
        }

        private static void BuildMeterPayload(
            string context,
            MetricValueSourceBase<MeterValue> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<MeterFields, string> fields,
            DateTime timestamp)
        {
            writer.WriteMeter(context, valueSource as MeterValueSource, fields, timestamp);
        }

        private static void BuildMetricPayload<TMetric>(
            string context,
            MetricValueSourceBase<TMetric> valueSource,
            IMetricSnapshotWriter writer,
            MetricFields fields,
            DateTime timestamp)
        {
            if (typeof(TMetric) == typeof(double))
            {
                BuildGaugePayload(context, valueSource as MetricValueSourceBase<double>, writer, fields.Gauge, timestamp);
                return;
            }

            if (typeof(TMetric) == typeof(CounterValue))
            {
                BuildCounterPayload(context, valueSource as MetricValueSourceBase<CounterValue>, writer, fields.Counter, timestamp);
                return;
            }

            if (typeof(TMetric) == typeof(MeterValue))
            {
                BuildMeterPayload(context, valueSource as MetricValueSourceBase<MeterValue>, writer, fields.Meter, timestamp);
                return;
            }

            if (typeof(TMetric) == typeof(TimerValue))
            {
                BuildTimerPayload(context, valueSource as MetricValueSourceBase<TimerValue>, writer, fields.Meter, fields.Histogram, timestamp);
                return;
            }

            if (typeof(TMetric) == typeof(BucketTimerValue))
            {
                BuildBucketTimerPayload(context, valueSource as MetricValueSourceBase<BucketTimerValue>, writer, fields.Histogram.ToDictionary(x => x.Key.ToString(), x => x.Value), timestamp);
                return;
            }

            if (typeof(TMetric) == typeof(HistogramValue))
            {
                BuildHistogramPayload(context, valueSource as MetricValueSourceBase<HistogramValue>, writer, fields.Histogram, timestamp);
                return;
            }

            if (typeof(TMetric) == typeof(BucketHistogramValue))
            {
                BuildBucketHistogramPayload(context, valueSource as MetricValueSourceBase<BucketHistogramValue>, writer, fields.BucketHistogram.ToDictionary(x => x.Key.ToString(), x => x.Value), timestamp);
                return;
            }

            if (typeof(TMetric) == typeof(ApdexValue))
            {
                BuildApdexPayload(context, valueSource as MetricValueSourceBase<ApdexValue>, writer, fields.Apdex, timestamp);
            }
        }

        private static void BuildTimerPayload(
            string context,
            MetricValueSourceBase<TimerValue> valueSource,
            IMetricSnapshotWriter writer,
            IDictionary<MeterFields, string> meterFields,
            IDictionary<HistogramFields, string> histogramFields,
            DateTime timestamp)
        {
            writer.WriteTimer(context, valueSource, meterFields, histogramFields, timestamp);
        }
    }
}