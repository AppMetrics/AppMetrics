// <copyright file="Apdex.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class Apdex : IClassFixture<MetricsCoreTestFixture>
    {
        private readonly MetricsCoreTestFixture _fixture;

        public Apdex(MetricsCoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void AlorithmR()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Apdex.Track(
                        MetricOptions.Apdex.OptionsAlgorithmR,
                        _fixture.ActionToTrack);
                });
        }

        [Fact]
        public void ForwardDecaying()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Apdex.Track(
                        MetricOptions.Apdex.OptionsForwardDecaying,
                        () => { _fixture.ActionToTrack(); });
                });
        }

        [Fact]
        public void SlidingWindow()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Apdex.Track(
                        MetricOptions.Apdex.OptionsSlidingWindow,
                        _fixture.ActionToTrack);
                });
        }
    }
}