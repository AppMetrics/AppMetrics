// <copyright file="MeasureTimerBenchmark.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureTimerBenchmark : DefaultBenchmarkBase
    {
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