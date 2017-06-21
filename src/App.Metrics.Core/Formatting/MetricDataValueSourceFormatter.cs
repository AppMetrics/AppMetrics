// <copyright file="MetricDataValueSourceFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Reporting;
using App.Metrics.Timer;

namespace App.Metrics.Core.Formatting
{
    public class MetricDataValueSourceFormatter
    {
        public void Build<TPayload>(MetricsDataValueSource metricsData, IMetricPayloadBuilder<TPayload> payloadBuilder)
        {
            foreach (var contextValueSource in metricsData.Contexts)
            {
                foreach (var valueSource in contextValueSource.ApdexScores)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, payloadBuilder);
                }

                foreach (var valueSource in contextValueSource.Gauges)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, payloadBuilder);
                }

                foreach (var valueSource in contextValueSource.Counters)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, payloadBuilder);
                }

                foreach (var valueSource in contextValueSource.Meters)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, payloadBuilder);
                }

                foreach (var valueSource in contextValueSource.Timers)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, payloadBuilder);
                }

                foreach (var valueSource in contextValueSource.Histograms)
                {
                    BuildMetricPayload(contextValueSource.Context, valueSource, payloadBuilder);
                }
            }
        }

        private static void BuildApdexPayload<T>(
            string context,
            MetricValueSourceBase<ApdexValue> valueSource,
            IMetricPayloadBuilder<T> payloadBuilder)
        {
            payloadBuilder.PackApdex(context, valueSource);
        }

        private static void BuildCounterPayload<T>(
            string context,
            MetricValueSourceBase<CounterValue> valueSource,
            IMetricPayloadBuilder<T> payloadBuilder)
        {
            var counterValueSource = valueSource as CounterValueSource;
            payloadBuilder.PackCounter(context, valueSource, counterValueSource);
        }

        private static void BuildGaugePayload<T>(string context, MetricValueSourceBase<double> valueSource, IMetricPayloadBuilder<T> payloadBuilder)
        {
            payloadBuilder.PackGauge(context, valueSource);
        }

        private static void BuildHistogramPayload<T>(
            string context,
            MetricValueSourceBase<HistogramValue> valueSource,
            IMetricPayloadBuilder<T> payloadBuilder)
        {
            payloadBuilder.PackHistogram(context, valueSource);
        }

        private static void BuildMeterPayload<T>(
            string context,
            MetricValueSourceBase<MeterValue> valueSource,
            IMetricPayloadBuilder<T> payloadBuilder)
        {
            payloadBuilder.PackMeter(context, valueSource);
        }

        private static void BuildMetricPayload<TMetric, TPayload>(
            string context,
            MetricValueSourceBase<TMetric> valueSource,
            IMetricPayloadBuilder<TPayload> payloadBuilder)
        {
            if (typeof(TMetric) == typeof(double))
            {
                BuildGaugePayload(context, valueSource as MetricValueSourceBase<double>, payloadBuilder);
                return;
            }

            if (typeof(TMetric) == typeof(CounterValue))
            {
                BuildCounterPayload(context, valueSource as MetricValueSourceBase<CounterValue>, payloadBuilder);
                return;
            }

            if (typeof(TMetric) == typeof(MeterValue))
            {
                BuildMeterPayload(context, valueSource as MetricValueSourceBase<MeterValue>, payloadBuilder);
                return;
            }

            if (typeof(TMetric) == typeof(TimerValue))
            {
                BuildTimerPayload(context, valueSource as MetricValueSourceBase<TimerValue>, payloadBuilder);
                return;
            }

            if (typeof(TMetric) == typeof(HistogramValue))
            {
                BuildHistogramPayload(context, valueSource as MetricValueSourceBase<HistogramValue>, payloadBuilder);
                return;
            }

            if (typeof(TMetric) == typeof(ApdexValue))
            {
                BuildApdexPayload(context, valueSource as MetricValueSourceBase<ApdexValue>, payloadBuilder);
            }
        }

        private static void BuildTimerPayload<T>(
            string context,
            MetricValueSourceBase<TimerValue> valueSource,
            IMetricPayloadBuilder<T> payloadBuilder)
        {
            payloadBuilder.PackTimer(context, valueSource);
        }
    }
}