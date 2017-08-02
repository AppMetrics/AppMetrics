// <copyright file="ApplicationsMetricsRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace MetricsSandbox
{
    public static class ApplicationsMetricsRegistry
    {
        public static ApdexOptions ApdexOne => new ApdexOptions
                                               {
                                                   Name = "apdex_one"
                                               };

        public static CounterOptions CounterOne => new CounterOptions
                                                   {
                                                       Name = "counter_one"
                                                   };

        public static GaugeOptions GaugeOne => new GaugeOptions
                                               {
                                                   Name = "gauge_one"
                                               };

        public static HistogramOptions HistogramOne => new HistogramOptions
                                                       {
                                                           Name = "histogram_one"
                                                       };

        public static MeterOptions MeterOne => new MeterOptions
                                               {
                                                   Name = "meter_one"
                                               };

        public static TimerOptions TimerOne => new TimerOptions
                                               {
                                                   Name = "timer_one"
                                               };
    }
}