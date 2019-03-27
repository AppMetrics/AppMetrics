// <copyright file="Gauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

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
        public void SetValue()
        {
            SimpleBenchmarkRunner.Run(
                () => { _fixture.Metrics.Measure.Gauge.SetValue(MetricOptions.Gauge.Options, () => _fixture.Rnd.NextDouble()); });
        }

        [Fact]
        public void SetValueNotLazy()
        {
            SimpleBenchmarkRunner.Run(
                () => { _fixture.Metrics.Measure.Gauge.SetValue(MetricOptions.Gauge.OptionsNotLazy, _fixture.Rnd.NextDouble()); });
        }
    }
}