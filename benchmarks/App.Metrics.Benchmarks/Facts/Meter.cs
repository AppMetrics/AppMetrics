// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using App.Metrics.Tagging;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class Meter : IClassFixture<MetricsCoreTestFixture>
    {
        private readonly MetricsCoreTestFixture _fixture;

        public Meter(MetricsCoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void Mark()
        {
            SimpleBenchmarkRunner.Run(
                () => { _fixture.Metrics.Measure.Meter.Mark(MetricOptions.Meter.Options); });
        }

        [Fact]
        public void MarkMetricItem()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Meter.Mark(
                        MetricOptions.Meter.OptionsWithMetricItem,
                        new MetricSetItem("key", "value"));
                });
        }

        [Fact]
        public void MarkUserValue()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Meter.Mark(
                        MetricOptions.Meter.OptionsWithUserValue,
                        _fixture.Rnd.Next(0, 1000),
                        _fixture.RandomUserValue);
                });
        }
    }
}