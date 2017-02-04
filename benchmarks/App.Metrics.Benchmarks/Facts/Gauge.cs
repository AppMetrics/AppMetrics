// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class Gauge : IClassFixture<MetricsCoreTestFixture>
    {
        private readonly MetricsCoreTestFixture _fixture;

        public Gauge(MetricsCoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void Increment()
        {
            SimpleBenchmarkRunner.Run(
                () => { _fixture.Metrics.Measure.Gauge.SetValue(MetricOptions.Gauge.Options, () => _fixture.Rnd.NextDouble()); });
        }
    }
}