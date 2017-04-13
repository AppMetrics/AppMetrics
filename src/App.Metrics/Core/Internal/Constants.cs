// <copyright file="Constants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Health;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Reporting;
using App.Metrics.Timer;

namespace App.Metrics.Core.Internal
{
    internal static class Constants
    {
        public const string InternalMetricsContext = "appmetrics.internal";

        public static class DataKeyMapping
        {
            public static IDictionary<ApdexValueDataKeys, string> Apdex => new Dictionary<ApdexValueDataKeys, string>
                                                                                                {
                                                                                                    { ApdexValueDataKeys.Samples, "samples" },
                                                                                                    { ApdexValueDataKeys.Score, "score" },
                                                                                                    { ApdexValueDataKeys.Satisfied, "satisfied" },
                                                                                                    { ApdexValueDataKeys.Tolerating, "tolerating" },
                                                                                                    { ApdexValueDataKeys.Frustrating, "frustrating" }
                                                                                                };

            public static IDictionary<HistogramValueDataKeys, string> Histogram => new Dictionary<HistogramValueDataKeys, string>
                                                                                                   {
                                                                                                       { HistogramValueDataKeys.Samples, "samples" },
                                                                                                       { HistogramValueDataKeys.LastValue, "last" },
                                                                                                       { HistogramValueDataKeys.Sum, "sum" },
                                                                                                       { HistogramValueDataKeys.Count, "count.hist" },
                                                                                                       { HistogramValueDataKeys.Min, "min" },
                                                                                                       { HistogramValueDataKeys.Max, "max" },
                                                                                                       { HistogramValueDataKeys.Mean, "mean" },
                                                                                                       { HistogramValueDataKeys.Median, "median" },
                                                                                                       { HistogramValueDataKeys.StdDev, "stddev" },
                                                                                                       { HistogramValueDataKeys.P999, "p999" },
                                                                                                       { HistogramValueDataKeys.P99, "p99" },
                                                                                                       { HistogramValueDataKeys.P98, "p98" },
                                                                                                       { HistogramValueDataKeys.P95, "p95" },
                                                                                                       { HistogramValueDataKeys.P75, "p75" },
                                                                                                       {
                                                                                                           HistogramValueDataKeys.UserLastValue,
                                                                                                           "user.last"
                                                                                                       },
                                                                                                       { HistogramValueDataKeys.UserMinValue, "user.min" },
                                                                                                       { HistogramValueDataKeys.UserMaxValue, "user.max" }
                                                                                                   };

            public static IDictionary<MeterValueDataKeys, string> Meter => new Dictionary<MeterValueDataKeys, string>
                                                                                                {
                                                                                                    { MeterValueDataKeys.Count, "count.meter" },
                                                                                                    { MeterValueDataKeys.Rate1M, "rate1m" },
                                                                                                    { MeterValueDataKeys.Rate5M, "rate5m" },
                                                                                                    { MeterValueDataKeys.Rate15M, "rate15m" },
                                                                                                    { MeterValueDataKeys.RateMean, "rate.mean" },
                                                                                                    { MeterValueDataKeys.SetItemPercent, "percent" }
                                                                                                };

            public static IDictionary<CounterValueDataKeys, string> Counter => new Dictionary<CounterValueDataKeys, string>
                                                                           {
                                                                               { CounterValueDataKeys.Total, "total" },
                                                                               { CounterValueDataKeys.SetItemPercent, "percent" }
                                                                           };
        }

        public static class Formatting
        {
            public static readonly string MetricNameDimensionSeparator = "|";
            public static readonly string MetricSetItemFallbackKey = "item";
            public static readonly char MetricSetItemKeyValueSeparator = ':';
            public static readonly char MetricSetItemSeparator = ',';
            public static readonly string MetricTagKeyValueSeparator = ":";
            public static readonly string MetricTagSeparator = ",";
        }

        public static class Health
        {
#pragma warning disable SA1008
            public static readonly (double healthy, double degraded, double unhealthy) HealthScore = (1.0, 0.5, 0.0);
#pragma warning restore SA1008

            internal const string DegradedStatusDisplay = "Degraded";
            internal const string HealthyStatusDisplay = "Healthy";
            internal const string UnhealthyStatusDisplay = "Unhealthy";

            public static ReadOnlyDictionary<HealthCheckStatus, string> HealthStatusDisplay =>
                new ReadOnlyDictionary<HealthCheckStatus, string>(
                    new Dictionary<HealthCheckStatus, string>
                    {
                        { HealthCheckStatus.Healthy, HealthyStatusDisplay },
                        { HealthCheckStatus.Unhealthy, UnhealthyStatusDisplay },
                        { HealthCheckStatus.Degraded, DegradedStatusDisplay },
                    });

            public static class TagKeys
            {
                public const string HealthCheckName = "health_check_name";
                public const string HealthCheckStatus = "health_check_status";
            }
        }

        public static class Pack
        {
            public static readonly string ApdexMetricTypeValue = "apdex";
            public static readonly string CounterMetricTypeValue = "counter";
            public static readonly string GaugeMetricTypeValue = "gauge";
            public static readonly string HistogramMetricTypeValue = "histogram";
            public static readonly string MeterMetricTypeValue = "meter";
            public static readonly string MetricSetItemSuffix = "  items";
            public static readonly string MetricTagsTypeKey = "mtype";

            public static readonly string TimerMetricTypeValue = "timer";

            public static readonly Dictionary<Type, string> MetricValueSourceTypeMapping = new Dictionary<Type, string>
                                                                                           {
                                                                                               { typeof(double), GaugeMetricTypeValue },
                                                                                               { typeof(CounterValue), CounterMetricTypeValue },
                                                                                               { typeof(MeterValue), MeterMetricTypeValue },
                                                                                               { typeof(TimerValue), TimerMetricTypeValue },
                                                                                               { typeof(HistogramValue), HistogramMetricTypeValue },
                                                                                               { typeof(ApdexValue), ApdexMetricTypeValue }
                                                                                           };
        }

        public static class ReservoirSampling
        {
            public const int ApdexRequiredSamplesBeforeCalculating = 100;
            public const double DefaultApdexTSeconds = 0.5;
            public const double DefaultExponentialDecayFactor = 0.015;
            public const int DefaultSampleSize = 1028;
        }
    }
}