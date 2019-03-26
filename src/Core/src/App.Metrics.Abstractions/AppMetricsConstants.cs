// <copyright file="AppMetricsConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics
{
    public static class AppMetricsConstants
    {
        public const string InternalMetricsContext = "appmetrics.internal";

        public static class Pack
        {
            public static readonly string ApdexMetricTypeValue = "apdex";
            public static readonly string CounterMetricTypeValue = "counter";
            public static readonly string GaugeMetricTypeValue = "gauge";
            public static readonly string HistogramMetricTypeValue = "histogram";
            public static readonly string MeterMetricTypeValue = "meter";
            public static readonly string TimerMetricTypeValue = "timer";
            public static readonly string MetricTagsTypeKey = "mtype";
            public static readonly string MetricTagsUnitKey = "unit";
            public static readonly string MetricTagsUnitRateKey = "unit_rate";
            public static readonly string MetricTagsUnitRateDurationKey = "unit_dur";

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

        public static class Reporting
        {
            public static readonly TimeSpan DefaultFlushInterval = TimeSpan.FromSeconds(5);
        }
    }
}