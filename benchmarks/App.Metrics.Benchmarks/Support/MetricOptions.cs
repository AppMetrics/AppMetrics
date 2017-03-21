// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics;
using App.Metrics.Core.Options;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;

namespace App.Metrics.Benchmarks.Support
{
    public static class MetricOptions
    {
        public static class Apdex
        {
            public static readonly ApdexOptions OptionsAlgorithmR = new ApdexOptions
                                                                    {
                                                                        Context = nameof(MeasureApdexBenchmark),
                                                                        Name = "test_apdex_algorithmr",
                                                                        Reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir()),
                                                                        MeasurementUnit = Unit.Results,
                                                                        AllowWarmup = false
                                                                    };

            public static readonly ApdexOptions OptionsForwardDecaying = new ApdexOptions
                                                                         {
                                                                             Context = nameof(MeasureApdexBenchmark),
                                                                             Name = "test_apdex_forwarddecaying",
                                                                             Reservoir =
                                                                                 new Lazy<IReservoir>(() => new DefaultForwardDecayingReservoir()),
                                                                             MeasurementUnit = Unit.Results,
                                                                             AllowWarmup = false
                                                                         };

            public static readonly ApdexOptions OptionsSlidingWindow = new ApdexOptions
                                                                       {
                                                                           Context = nameof(MeasureApdexBenchmark),
                                                                           Name = "test_apdex_slidingwindow",
                                                                           Reservoir = new Lazy<IReservoir>(() => new DefaultSlidingWindowReservoir()),
                                                                           MeasurementUnit = Unit.Results,
                                                                           AllowWarmup = false
                                                                       };
        }

        public static class Counter
        {
            public static readonly CounterOptions Options = new CounterOptions
                                                            {
                                                                Context = nameof(MeasureCounterWithMetricItemBenchmark),
                                                                Name = "test_counter"
                                                            };

            public static readonly CounterOptions OptionsWithMetricItem = new CounterOptions
                                                                          {
                                                                              Context = nameof(MeasureCounterWithMetricItemBenchmark),
                                                                              Name = "test_counter_with_metric_item"
                                                                          };

            public static readonly CounterOptions OptionsWithUserValue = new CounterOptions
                                                                         {
                                                                             Context = nameof(MeasureCounterWithMetricItemBenchmark),
                                                                             Name = "test_counter_with_user_value"
                                                                         };
        }

        public static class Gauge
        {
            public static readonly GaugeOptions Options = new GaugeOptions
                                                          {
                                                              Context = nameof(MeasureGaugeBenchmark),
                                                              Name = "test_gauge"
                                                          };

            public static readonly GaugeOptions OptionsNotLazy = new GaugeOptions
                                                          {
                                                              Context = nameof(MeasureGaugeBenchmark),
                                                              Name = "test_gauge_not_lazy"
                                                          };
        }

        public static class Histogram
        {
            public static readonly HistogramOptions OptionsAlgorithmR = new HistogramOptions
                                                                        {
                                                                            Context = nameof(MeasureHistogramBenchmark),
                                                                            Name = "test_histogram_algorithmr",
                                                                            Reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir())
                                                                        };

            public static readonly HistogramOptions OptionsForwardDecaying = new HistogramOptions
                                                                             {
                                                                                 Context = nameof(MeasureHistogramBenchmark),
                                                                                 Name = "test_histogram_forwarddecaying",
                                                                                 Reservoir =
                                                                                     new Lazy<IReservoir>(() => new DefaultForwardDecayingReservoir())
                                                                             };

            public static readonly HistogramOptions OptionsSlidingWindow = new HistogramOptions
                                                                           {
                                                                               Context = nameof(MeasureHistogramBenchmark),
                                                                               Name = "test_histogram_slidingwindow",
                                                                               Reservoir =
                                                                                   new Lazy<IReservoir>(() => new DefaultSlidingWindowReservoir())
                                                                           };
        }

        public static class Meter
        {
            public static readonly MeterOptions Options = new MeterOptions
                                                          {
                                                              Context = nameof(MeasureMeterBenchmark),
                                                              Name = "test_meter"
                                                          };

            public static readonly MeterOptions OptionsWithMetricItem = new MeterOptions
                                                                        {
                                                                            Context = nameof(MeasureMeterBenchmark),
                                                                            Name = "test_meter_with_metric_item"
                                                                        };

            public static readonly MeterOptions OptionsWithUserValue = new MeterOptions
                                                                       {
                                                                           Context = nameof(MeasureMeterBenchmark),
                                                                           Name = "test_meter_with_user_value"
                                                                       };
        }

        public static class Timer
        {
            public static readonly TimerOptions OptionsAlgorithmR = new TimerOptions
                                                                    {
                                                                        Context = nameof(MeasureTimerBenchmark),
                                                                        Name = "test_timer_algorithmr",
                                                                        Reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir()),
                                                                        DurationUnit = TimeUnit.Milliseconds,
                                                                        MeasurementUnit = Unit.Results
                                                                    };

            public static readonly TimerOptions OptionsForwardDecaying = new TimerOptions
                                                                         {
                                                                             Context = nameof(MeasureTimerBenchmark),
                                                                             Name = "test_timer_forwarddecaying",
                                                                             Reservoir =
                                                                                 new Lazy<IReservoir>(() => new DefaultForwardDecayingReservoir()),
                                                                             DurationUnit = TimeUnit.Milliseconds,
                                                                             MeasurementUnit = Unit.Results
                                                                         };

            public static readonly TimerOptions OptionsSlidingWindow = new TimerOptions
                                                                       {
                                                                           Context = nameof(MeasureTimerBenchmark),
                                                                           Name = "test_timer_slidingwindow",
                                                                           Reservoir = new Lazy<IReservoir>(() => new DefaultSlidingWindowReservoir()),
                                                                           DurationUnit = TimeUnit.Milliseconds,
                                                                           MeasurementUnit = Unit.Results
                                                                       };
        }
    }
}