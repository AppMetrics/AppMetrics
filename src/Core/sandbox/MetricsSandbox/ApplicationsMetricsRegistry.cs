// <copyright file="ApplicationsMetricsRegistry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Timer;

namespace MetricsSandbox
{
    public static class ApplicationsMetricsRegistry
    {
        public static ApdexOptions ApdexOne => new ApdexOptions
                                               {
                                                   Name = "apdex_one",
                                                   AllowWarmup = false,
                                                   ApdexTSeconds = 0.1,
                                                   Reservoir = () => new DefaultAlgorithmRReservoir()
                                               };

        public static CounterOptions CounterOne => new CounterOptions
                                                   {
                                                       Name = "counter_one"
                                                   };

        public static CounterOptions CounterWithSetItems => new CounterOptions
                                                   {
                                                       Name = "counter_withitems",
                                                       ReportSetItems = false
                                                   };

        public static GaugeOptions GaugeOne => new GaugeOptions
                                               {
                                                   Name = "gauge_one"
                                               };

        public static HistogramOptions HistogramOne => new HistogramOptions
                                                       {
                                                           Name = "histogram_one"
                                                       };

        public static BucketHistogramOptions BucketHistogramOne => new BucketHistogramOptions
                                                                    {
                                                                        Name = "bucket_histogram_one",
                                                                        Buckets = new []{10d,50d,100d}
                                                                    };

        public static MeterOptions MeterOne => new MeterOptions
                                               {
                                                   Name = "meter_one"
                                               };

        public static MeterOptions MeterWithSetItems => new MeterOptions
                                                            {
                                                                Name = "meter_withitems",
                                                                ReportSetItems = false
                                                            };

        public static TimerOptions TimerOne => new TimerOptions
                                               {
                                                   Name = "timer_one"
                                               };

        public static BucketTimerOptions BucketTimerOne => new BucketTimerOptions
        {
            Name = "timer_one"
        };
    }
}