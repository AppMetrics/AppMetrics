// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class Timer : IClassFixture<MetricsCoreTestFixture>
    {
        private readonly MetricsCoreTestFixture _fixture;

        public Timer(MetricsCoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void AlorithmR()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Timer.Time(
                        MetricOptions.Timer.OptionsAlgorithmR,
                        _fixture.ActionToTrack,
                        _fixture.RandomUserValue);
                });
        }

        [Fact]
        public void ForwardDecaying()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Timer.Time(
                        MetricOptions.Timer.OptionsForwardDecaying,
                        _fixture.ActionToTrack,
                        _fixture.RandomUserValue);
                });
        }

        [Fact]
        public void SlidingWindow()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Timer.Time(
                        MetricOptions.Timer.OptionsSlidingWindow,
                        _fixture.ActionToTrack,
                        _fixture.RandomUserValue);
                });
        }
    }
}