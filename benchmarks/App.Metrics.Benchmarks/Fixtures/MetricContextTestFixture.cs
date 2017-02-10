// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Counter;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge;
using App.Metrics.Gauge.Abstractions;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Registry.Internal;
using App.Metrics.ReservoirSampling;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Benchmarks.Fixtures
{
    public class MetricContextTestFixture : IDisposable
    {
        public MetricContextTestFixture()
        {
            ApdexOptions = new ApdexOptions
                           {
                               Name = "apdex"
                           };

            CounterOptions = new CounterOptions
                             {
                                 Name = "counter"
                             };

            GaugeOptions = new GaugeOptions
                           {
                               Name = "gauge"
                           };

            HistogramOptions = new HistogramOptions
                               {
                                   Name = "histogram"
                               };

            MeterOptions = new MeterOptions
                           {
                               Name = "meter"
                           };

            TimerOptions = new TimerOptions
                           {
                               Name = "timer"
                           };

            var tags = new GlobalMetricTags
                       {
                           { "key1", "value1" },
                           { "key2", "value2" }
                       };

            Registry = new DefaultMetricContextRegistry("context_label", tags);
            ApdexBuilder = new DefaultApdexBuilder(new DefaultSamplingReservoirProvider());
            HistogramBuilder = new DefaultHistogramBuilder(new DefaultSamplingReservoirProvider());
            CounterBuilder = new DefaultCounterBuilder();
            GaugeBuilder = new DefaultGaugeBuilder();
            MeterBuilder = new DefaultMeterBuilder();
            TimerBuilder = new DefaultTimerBuilder(new DefaultSamplingReservoirProvider());
            Clock = new StopwatchClock();
        }

        public IBuildApdexMetrics ApdexBuilder { get; }

        public ApdexOptions ApdexOptions { get; }

        public IClock Clock { get; }

        public IBuildCounterMetrics CounterBuilder { get; }

        public CounterOptions CounterOptions { get; }

        public IBuildGaugeMetrics GaugeBuilder { get; }

        public GaugeOptions GaugeOptions { get; }

        public IBuildHistogramMetrics HistogramBuilder { get; }

        public HistogramOptions HistogramOptions { get; }

        public IBuildMeterMetrics MeterBuilder { get; }

        public MeterOptions MeterOptions { get; }

        public IMetricContextRegistry Registry { get; }

        public IBuildTimerMetrics TimerBuilder { get; }

        public TimerOptions TimerOptions { get; }

        public void Dispose() { }
    }
}