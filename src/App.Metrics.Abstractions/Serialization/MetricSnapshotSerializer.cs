// <copyright file="MetricSnapshotSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
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
        public void Serialize(IMetricSnapshotWriter writer, MetricsDataValueSource metricsData)
        {
            foreach (var contextValueSource in metricsData.Contexts)
            {
                foreach (var valueSource in contextValueSource.ApdexScores)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer);
                }

                foreach (var valueSource in contextValueSource.Gauges)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer);
                }

                foreach (var valueSource in contextValueSource.Counters)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer);
                }

                foreach (var valueSource in contextValueSource.Meters)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer);
                }

                foreach (var valueSource in contextValueSource.Timers)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer);
                }

                foreach (var valueSource in contextValueSource.Histograms)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, writer);
                }
            }
        }

        private static void BuildApdexPayload(
            string context,
            MetricValueSourceBase<ApdexValue> valueSource,
            IMetricSnapshotWriter writer)
        {
            writer.WriteApdex(context, valueSource);
        }

        private static void BuildCounterPayload(
            string context,
            MetricValueSourceBase<CounterValue> valueSource,
            IMetricSnapshotWriter writer)
        {
            var counterValueSource = valueSource as CounterValueSource;
            writer.WriteCounter(context, valueSource, counterValueSource);
        }

        private static void BuildGaugePayload(string context, MetricValueSourceBase<double> valueSource, IMetricSnapshotWriter writer)
        {
            writer.WriteGauge(context, valueSource);
        }

        private static void BuildHistogramPayload(
            string context,
            MetricValueSourceBase<HistogramValue> valueSource,
            IMetricSnapshotWriter writer)
        {
            writer.WriteHistogram(context, valueSource);
        }

        private static void BuildMeterPayload(
            string context,
            MetricValueSourceBase<MeterValue> valueSource,
            IMetricSnapshotWriter writer)
        {
            writer.WriteMeter(context, valueSource);
        }

        private static void BuildMetricPayload<TMetric>(
            string context,
            MetricValueSourceBase<TMetric> valueSource,
            IMetricSnapshotWriter writer)
        {
            if (typeof(TMetric) == typeof(double))
            {
                BuildGaugePayload(context, valueSource as MetricValueSourceBase<double>, writer);
                return;
            }

            if (typeof(TMetric) == typeof(CounterValue))
            {
                BuildCounterPayload(context, valueSource as MetricValueSourceBase<CounterValue>, writer);
                return;
            }

            if (typeof(TMetric) == typeof(MeterValue))
            {
                BuildMeterPayload(context, valueSource as MetricValueSourceBase<MeterValue>, writer);
                return;
            }

            if (typeof(TMetric) == typeof(TimerValue))
            {
                BuildTimerPayload(context, valueSource as MetricValueSourceBase<TimerValue>, writer);
                return;
            }

            if (typeof(TMetric) == typeof(HistogramValue))
            {
                BuildHistogramPayload(context, valueSource as MetricValueSourceBase<HistogramValue>, writer);
                return;
            }

            if (typeof(TMetric) == typeof(ApdexValue))
            {
                BuildApdexPayload(context, valueSource as MetricValueSourceBase<ApdexValue>, writer);
            }
        }

        private static void BuildTimerPayload(
            string context,
            MetricValueSourceBase<TimerValue> valueSource,
            IMetricSnapshotWriter writer)
        {
            writer.WriteTimer(context, valueSource);
        }
    }
}