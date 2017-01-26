// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultMetricsBuilderFactoryTests
    {
        private readonly DefaultMetricsBuilderFactory _factory;

        public DefaultMetricsBuilderFactoryTests() { _factory = new DefaultMetricsBuilderFactory(); }

        [Fact]
        public void creates_apdex_builder() { _factory.Apdex.Should().NotBeNull(); }

        [Fact]
        public void creates_counter_builder() { _factory.Counter.Should().NotBeNull(); }

        [Fact]
        public void creates_gauge_builder() { _factory.Gauge.Should().NotBeNull(); }

        [Fact]
        public void creates_histogram_builder() { _factory.Histogram.Should().NotBeNull(); }

        [Fact]
        public void creates_meter_builder() { _factory.Meter.Should().NotBeNull(); }

        [Fact]
        public void creates_timer_builder() { _factory.Timer.Should().NotBeNull(); }
    }
}