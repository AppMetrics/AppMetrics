// <copyright file="MetricContextRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class MetricContextRegistry : IClassFixture<MetricContextTestFixture>
    {
        private readonly MetricContextTestFixture _fixture;

        public MetricContextRegistry(MetricContextTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void Resolve_apdex_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Registry.Apdex(
                        _fixture.ApdexOptions,
                        () =>
                            _fixture.ApdexBuilder.Build(
                                _fixture.ApdexOptions.ApdexTSeconds,
                                _fixture.ApdexOptions.AllowWarmup,
                                _fixture.Clock));
                });
        }

        [Fact]
        public void Resolve_counter_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Registry.Counter(
                        _fixture.CounterOptions,
                        () => _fixture.CounterBuilder.Build());
                });
        }

        [Fact]
        public void Resolve_gauge_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Registry.Gauge(
                        _fixture.GaugeOptions,
                        () => _fixture.GaugeBuilder.Build(() => 1.0));
                });
        }

        [Fact]
        public void Resolve_histogram_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Registry.Histogram(
                        _fixture.HistogramOptions,
                        () =>
                            _fixture.HistogramBuilder.Build(_fixture.HistogramOptions.Reservoir));
                });
        }

        [Fact]
        public void Resolve_meter_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Registry.Meter(
                        _fixture.MeterOptions,
                        () =>
                            _fixture.MeterBuilder.Build(_fixture.Clock));
                });
        }

        [Fact]
        public void Resolve_timer_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    _fixture.Registry.Timer(
                        _fixture.TimerOptions,
                        () =>
                            _fixture.TimerBuilder.Build(
                                _fixture.TimerOptions.Reservoir,
                                _fixture.Clock));
                });
        }
    }
}