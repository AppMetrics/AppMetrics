// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Facts.Fixtures;
using App.Metrics.Internal.Managers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultMetricsManagerFactoryTests : IClassFixture<MetricManagerTestFixture>
    {
        private readonly DefaultMetricsManagerFactory _factory;

        public DefaultMetricsManagerFactoryTests(MetricManagerTestFixture fixture)
        {
            _factory = new DefaultMetricsManagerFactory(fixture.Registry, fixture.Builder, fixture.Clock);
        }

        [Fact]
        public void creates_apdex_manager()
        {
            _factory.ApdexManager.Should().NotBeNull();
        }

        [Fact]
        public void creates_counter_manager()
        {
            _factory.CounterManager.Should().NotBeNull();
        }

        [Fact]
        public void creates_gauge_manager()
        {
            _factory.GaugeManager.Should().NotBeNull();
        }

        [Fact]
        public void creates_histogram_manager()
        {
            _factory.HistogramManager.Should().NotBeNull();
        }

        [Fact]
        public void creates_meter_manager()
        {
            _factory.MeterManager.Should().NotBeNull();
        }

        [Fact]
        public void creates_timer_manager()
        {
            _factory.TimerManager.Should().NotBeNull();
        }
    }
}