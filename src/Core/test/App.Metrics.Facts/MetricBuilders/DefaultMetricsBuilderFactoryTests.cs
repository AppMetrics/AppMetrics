// <copyright file="DefaultMetricsBuilderFactoryTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Internal;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.MetricBuilders
{
    public class DefaultMetricsBuilderFactoryTests
    {
        private readonly DefaultMetricsBuilderFactory _factory;

        public DefaultMetricsBuilderFactoryTests() { _factory = new DefaultMetricsBuilderFactory(new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir())); }

        [Fact]
        public void Creates_apdex_builder()
        {
            _factory.Apdex.Should().NotBeNull();
        }

        [Fact]
        public void Creates_counter_builder()
        {
            _factory.Counter.Should().NotBeNull();
        }

        [Fact]
        public void Creates_gauge_builder()
        {
            _factory.Gauge.Should().NotBeNull();
        }

        [Fact]
        public void Creates_histogram_builder()
        {
            _factory.Histogram.Should().NotBeNull();
        }

        [Fact]
        public void Creates_meter_builder()
        {
            _factory.Meter.Should().NotBeNull();
        }

        [Fact]
        public void Creates_timer_builder()
        {
            _factory.Timer.Should().NotBeNull();
        }
    }
}