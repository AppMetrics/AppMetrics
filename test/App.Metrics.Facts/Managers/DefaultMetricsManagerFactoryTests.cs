// <copyright file="DefaultMetricsManagerFactoryTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Facts.Fixtures;
using App.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultMetricsManagerFactoryTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly DefaultMeasureMetricsProvider _factory;

        public DefaultMetricsManagerFactoryTests(MetricCoreTestFixture fixture)
        {
            _factory = new DefaultMeasureMetricsProvider(fixture.Registry, fixture.Builder, fixture.Clock);
        }

        [Fact]
        public void Creates_apdex_manager()
        {
            _factory.Apdex.Should().NotBeNull();
        }

        [Fact]
        public void Creates_counter_manager()
        {
            _factory.Counter.Should().NotBeNull();
        }

        [Fact]
        public void Creates_gauge_manager()
        {
            _factory.Gauge.Should().NotBeNull();
        }

        [Fact]
        public void Creates_histogram_manager()
        {
            _factory.Histogram.Should().NotBeNull();
        }

        [Fact]
        public void Creates_meter_manager()
        {
            _factory.Meter.Should().NotBeNull();
        }

        [Fact]
        public void Creates_timer_manager()
        {
            _factory.Timer.Should().NotBeNull();
        }
    }
}