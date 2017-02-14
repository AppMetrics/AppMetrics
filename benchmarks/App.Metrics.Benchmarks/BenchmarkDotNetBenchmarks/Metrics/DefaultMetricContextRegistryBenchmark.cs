// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
using App.Metrics.Registry.Internal;
using App.Metrics.ReservoirSampling;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class DefaultMetricContextRegistryBenchmark : DefaultBenchmarkBase
    {
        private static readonly IBuildApdexMetrics ApdexBuilder = new DefaultApdexBuilder(new DefaultSamplingReservoirProvider());


        private static readonly ApdexOptions ApdexOptions = new ApdexOptions
                                                            {
                                                                Name = "apdex"
                                                            };

        private static readonly IClock Clock = new StopwatchClock();

        private static readonly IBuildCounterMetrics CounterBuilder = new DefaultCounterBuilder();

        private static readonly CounterOptions CounterOptions = new CounterOptions
                                                                {
                                                                    Name = "counter"
                                                                };

        private static readonly IBuildGaugeMetrics GaugeBuilder = new DefaultGaugeBuilder();

        private static readonly GaugeOptions GaugeOptions = new GaugeOptions
                                                            {
                                                                Name = "gauge"
                                                            };

        private static readonly IBuildHistogramMetrics HistogramBuilder = new DefaultHistogramBuilder(new DefaultSamplingReservoirProvider());

        private static readonly HistogramOptions HistogramOptions = new HistogramOptions
                                                                    {
                                                                        Name = "histogram"
                                                                    };

        private static readonly IBuildMeterMetrics MeterBuilder = new DefaultMeterBuilder();

        private static readonly MeterOptions MeterOptions = new MeterOptions
                                                            {
                                                                Name = "meter"
                                                            };

        private static readonly IBuildTimerMetrics TimerBuilder = new DefaultTimerBuilder(new DefaultSamplingReservoirProvider());

        private static readonly TimerOptions TimerOptions = new TimerOptions
                                                            {
                                                                Name = "timer"
                                                            };

        private DefaultMetricContextRegistry _registry;

        [Setup]
        public override void Setup()
        {
            var tags = new GlobalMetricTags
                       {
                           { "key1", "value1" },
                           { "key2", "value2" }
                       };

            _registry = new DefaultMetricContextRegistry("context_label", tags);
        }

        [Benchmark]
        public void ResolveApdexFromRegistry()
        {
            var metric = _registry.Apdex(
                ApdexOptions,
                () => ApdexBuilder.Build(ApdexOptions.ApdexTSeconds, ApdexOptions.AllowWarmup, Clock));
        }

        [Benchmark]
        public void ResolveCounterFromRegistry()
        {
            var metric = _registry.Counter(
                CounterOptions,
                () => CounterBuilder.Build());
        }

        [Benchmark]
        public void ResolveGaugeFromRegistry()
        {
            _registry.Gauge(
                GaugeOptions,
                () => GaugeBuilder.Build(() => 1.0));
        }

        [Benchmark]
        public void ResolveHistogramFromRegistry()
        {
            var metric = _registry.Histogram(
                HistogramOptions,
                () => HistogramBuilder.Build(HistogramOptions.Reservoir));
        }

        [Benchmark]
        public void ResolveMeterFromRegistry()
        {
            var metric = _registry.Meter(
                MeterOptions,
                () => MeterBuilder.Build(Clock));
        }

        [Benchmark]
        public void ResolveTimerFromRegistry()
        {
            var metric = _registry.Timer(
                TimerOptions,
                () => TimerBuilder.Build(TimerOptions.Reservoir, Clock));
        }
    }
}