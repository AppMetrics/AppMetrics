// <copyright file="MeasureTimerBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
using App.Metrics.Timer;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureTimerBenchmark : DefaultBenchmarkBase
    {
        private const int NumberOfMetrics = 1000;
        private static readonly TimerOptions[] Metrics;

        static MeasureTimerBenchmark()
        {
            Metrics = new TimerOptions[NumberOfMetrics];

            for (var i = 0; i < NumberOfMetrics; i++)
            {
                Metrics[i] = new TimerOptions {Name = $"metric_{i:D4}"};
            }
        }
        
        [Benchmark]
        public void Many()
        {
            for (var i = 0; i < NumberOfMetrics; i++)
            {
                using (Fixture.Metrics.Measure.Timer.Time(Metrics[i]))
                {
                }
            }
        }
        
        [Benchmark(Baseline = true)]
        public void TimeAlgorithmR()
        {
            Fixture.Metrics.Measure.Timer.Time(
                MetricOptions.Timer.OptionsAlgorithmR,
                Fixture.ActionToTrack,
                Fixture.RandomUserValue);
        }

        [Benchmark]
        public void TimeAlgorithmRUsingContext()
        {
            using (
                Fixture.Metrics.Measure.Timer.Time(
                    MetricOptions.Timer.OptionsAlgorithmR,
                    Fixture.RandomUserValue))
            {
                Fixture.ActionToTrack();
            }
        }

        [Benchmark]
        public void TimeForwardDecaying()
        {
            Fixture.Metrics.Measure.Timer.Time(
                MetricOptions.Timer.OptionsForwardDecaying,
                Fixture.ActionToTrack,
                Fixture.RandomUserValue);
        }

        [Benchmark]
        public void TimeForwardDecayingUsingContext()
        {
            using (
                Fixture.Metrics.Measure.Timer.Time(
                    MetricOptions.Timer.OptionsForwardDecaying,
                    Fixture.RandomUserValue))
            {
                Fixture.ActionToTrack();
            }
        }

        [Benchmark]
        public void TimeSlidingWindow()
        {
            Fixture.Metrics.Measure.Timer.Time(
                MetricOptions.Timer.OptionsSlidingWindow,
                Fixture.ActionToTrack,
                Fixture.RandomUserValue);
        }

        [Benchmark]
        public void TimeSlidingWindowUsingContext()
        {
            using (
                Fixture.Metrics.Measure.Timer.Time(
                    MetricOptions.Timer.OptionsSlidingWindow,
                    Fixture.RandomUserValue))
            {
                Fixture.ActionToTrack();
            }
        }
    }
}