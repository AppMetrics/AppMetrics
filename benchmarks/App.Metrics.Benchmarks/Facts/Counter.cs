// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using App.Metrics.Tagging;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class Counter : IClassFixture<MetricsCoreTestFixture>
    {
        private readonly MetricsCoreTestFixture _fixture;

        public Counter(MetricsCoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void Decrement()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Counter.Decrement(
                        MetricOptions.Counter.Options,
                        new MetricSetItem("key", "value"));
                });
        }

        [Fact]
        public void DecrementMetricItem()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Counter.Decrement(
                        MetricOptions.Counter.OptionsWithMetricItem,
                        new MetricSetItem("key", "value"));
                });
        }

        [Fact]
        public void DecrementUserValue()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Counter.Decrement(
                        MetricOptions.Counter.OptionsWithUserValue,
                        _fixture.Rnd.Next(0, 1000),
                        _fixture.RandomUserValue);
                });
        }

        [Fact]
        public void Increment()
        {
            SimpleBenchmarkRunner.Run(
                () => { _fixture.Metrics.Measure.Counter.Increment(MetricOptions.Counter.Options); });
        }

        [Fact]
        public void IncrementMetricItem()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Counter.Increment(
                        MetricOptions.Counter.OptionsWithMetricItem,
                        new MetricSetItem("key", "value"));
                });
        }

        [Fact]
        public void IncrementUserValue()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Metrics.Measure.Counter.Increment(
                        MetricOptions.Counter.OptionsWithUserValue,
                        _fixture.Rnd.Next(0, 1000),
                        _fixture.RandomUserValue);
                });
        }
    }
}