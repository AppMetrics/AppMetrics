// <copyright file="DefaultMetricContextRegistryBenchmark.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Timer;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class DefaultMetricContextRegistryBenchmark : DefaultBenchmarkBase
    {
        private static readonly IBuildApdexMetrics ApdexBuilder = new DefaultApdexBuilder(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir()));
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

        private static readonly IBuildHistogramMetrics HistogramBuilder = new DefaultHistogramBuilder(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir()));

        private static readonly HistogramOptions HistogramOptions = new HistogramOptions
                                                                    {
                                                                        Name = "histogram"
                                                                    };

        private static readonly IBuildMeterMetrics MeterBuilder = new DefaultMeterBuilder();

        private static readonly MeterOptions MeterOptions = new MeterOptions
                                                            {
                                                                Name = "meter"
                                                            };

        private static readonly IBuildTimerMetrics TimerBuilder = new DefaultTimerBuilder(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir()));

        private static readonly TimerOptions TimerOptions = new TimerOptions
                                                            {
                                                                Name = "timer"
                                                            };

        private DefaultMetricContextRegistry _registry;

        [GlobalSetup]
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
            _registry.Apdex(
                ApdexOptions,
                () => ApdexBuilder.Build(ApdexOptions.ApdexTSeconds, ApdexOptions.AllowWarmup, Clock));
        }

        [Benchmark]
        public void ResolveCounterFromRegistry()
        {
            _registry.Counter(
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
            _registry.Histogram(
                HistogramOptions,
                () => HistogramBuilder.Build(HistogramOptions.Reservoir));
        }

        [Benchmark]
        public void ResolveMeterFromRegistry()
        {
            _registry.Meter(
                MeterOptions,
                () => MeterBuilder.Build(Clock));
        }

        [Benchmark]
        public void ResolveTimerFromRegistry()
        {
            _registry.Timer(
                TimerOptions,
                () => TimerBuilder.Build(TimerOptions.Reservoir, Clock));
        }
    }
}