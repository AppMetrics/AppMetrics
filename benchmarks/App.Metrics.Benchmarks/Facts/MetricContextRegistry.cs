// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
        public void resolve_apdex_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    var metric = _fixture.Registry.Apdex(
                        _fixture.ApdexOptions,
                        () =>
                            _fixture.ApdexBuilder.Build(
                                _fixture.ApdexOptions.ApdexTSeconds,
                                _fixture.ApdexOptions.AllowWarmup,
                                _fixture.Clock));
                });
        }

        [Fact]
        public void resolve_counter_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    var metric = _fixture.Registry.Counter(
                        _fixture.CounterOptions,
                        () => _fixture.CounterBuilder.Build());
                });
        }

        [Fact]
        public void resolve_gauge_from_registry()
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
        public void resolve_histogram_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    var metric = _fixture.Registry.Histogram(
                        _fixture.HistogramOptions,
                        () =>
                            _fixture.HistogramBuilder.Build(_fixture.HistogramOptions.Reservoir));
                });
        }

        [Fact]
        public void resolve_meter_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    var metric = _fixture.Registry.Meter(
                        _fixture.MeterOptions,
                        () =>
                            _fixture.MeterBuilder.Build(_fixture.Clock));
                });
        }

        [Fact]
        public void resolve_timer_from_registry()
        {
            SimpleBenchmarkRunner.Run(
                () =>
                {
                    var metric = _fixture.Registry.Timer(
                        _fixture.TimerOptions,
                        () =>
                            _fixture.TimerBuilder.Build(
                                _fixture.TimerOptions.Reservoir,
                                _fixture.Clock));
                });
        }
    }
}