// <copyright file="MeasureApdexBenchmark.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Support;
using BenchmarkDotNet.Attributes;

namespace App.Metrics.Benchmarks.BenchmarkDotNetBenchmarks.Metrics
{
    public class MeasureApdexBenchmark : DefaultBenchmarkBase
    {
        [Benchmark]
        public void TimeAlgorithmR()
        {
            Fixture.Metrics.Measure.Apdex.Track(
                MetricOptions.Apdex.OptionsAlgorithmR,
                Fixture.ActionToTrack);
        }

        [Benchmark]
        public void TrackUsingAlgorithmRUsingContext()
        {
            using (Fixture.Metrics.Measure.Apdex.Track(MetricOptions.Apdex.OptionsAlgorithmR))
            {
                Fixture.ActionToTrack();
            }
        }

        [Benchmark]
        public void TrackUsingForwardDecaying()
        {
            Fixture.Metrics.Measure.Apdex.Track(
                MetricOptions.Apdex.OptionsForwardDecaying,
                () => { Fixture.ActionToTrack(); });
        }

        [Benchmark]
        public void TrackUsingForwardDecayingUsingContext()
        {
            using (Fixture.Metrics.Measure.Apdex.Track(MetricOptions.Apdex.OptionsForwardDecaying))
            {
                Fixture.ActionToTrack();
            }
        }

        [Benchmark]
        public void TrackUsingSlidingWindow()
        {
            Fixture.Metrics.Measure.Apdex.Track(
                MetricOptions.Apdex.OptionsSlidingWindow,
                Fixture.ActionToTrack);
        }

        [Benchmark]
        public void TrackUsingSlidingWindowUsingContext()
        {
            using (Fixture.Metrics.Measure.Apdex.Track(MetricOptions.Apdex.OptionsSlidingWindow))
            {
                Fixture.ActionToTrack();
            }
        }
    }
}