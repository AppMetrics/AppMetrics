// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class Histogram : IClassFixture<MetricsCoreTestFixture>
    {
        private readonly MetricsCoreTestFixture _fixture;

        public Histogram(MetricsCoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void AlorithmR()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Histogram.Update(
                        MetricOptions.Histogram.OptionsAlgorithmR,
                        _fixture.Rnd.Next(0, 1000),
                        _fixture.RandomUserValue);
                });
        }

        [Fact]
        public void ForwardDecaying()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Histogram.Update(
                        MetricOptions.Histogram.OptionsForwardDecaying,
                        _fixture.Rnd.Next(0, 1000),
                        _fixture.RandomUserValue);
                });
        }

        [Fact]
        public void SlidingWindow()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Histogram.Update(
                        MetricOptions.Histogram.OptionsSlidingWindow,
                        _fixture.Rnd.Next(0, 1000),
                        _fixture.RandomUserValue);
                });
        }
    }
}