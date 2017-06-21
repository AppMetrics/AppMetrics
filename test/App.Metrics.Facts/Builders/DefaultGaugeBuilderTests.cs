// <copyright file="DefaultGaugeBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.Gauge;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultGaugeBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildGaugeMetrics _builder;

        public DefaultGaugeBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Gauge; }

        [Fact]
        public void Can_build_custom_instance()
        {
            var counter = _builder.Build(() => new CustomGauge());

            counter.Should().NotBeNull();
            counter.Should().BeOfType<CustomGauge>();
        }

        [Fact]
        public void Can_build_new_instance()
        {
            var gauge = _builder.Build(() => 1.0);

            gauge.Should().NotBeNull();
        }
    }
}