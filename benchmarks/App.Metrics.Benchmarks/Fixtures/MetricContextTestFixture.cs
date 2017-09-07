// <copyright file="MetricContextTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Meter;
using App.Metrics.Registry;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Timer;

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

            var samplingProvider = new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir());

            Registry = new DefaultMetricContextRegistry("context_label", tags);
            ApdexBuilder = new DefaultApdexBuilder(samplingProvider);
            HistogramBuilder = new DefaultHistogramBuilder(samplingProvider);
            CounterBuilder = new DefaultCounterBuilder();
            GaugeBuilder = new DefaultGaugeBuilder();
            MeterBuilder = new DefaultMeterBuilder();
            TimerBuilder = new DefaultTimerBuilder(samplingProvider);
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