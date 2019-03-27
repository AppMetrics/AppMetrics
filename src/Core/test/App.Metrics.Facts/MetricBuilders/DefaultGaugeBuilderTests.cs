// <copyright file="DefaultGaugeBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.Gauge;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.MetricBuilders
{
    public class DefaultGaugeBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildGaugeMetrics _builder;

        public DefaultGaugeBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Gauge; }

        [Fact]
        public void Can_build_custom_instance()
        {
            // Arrange
            CustomGauge ValueProvider() => new CustomGauge();

            // Act
            var counter = _builder.Build((Func<CustomGauge>)ValueProvider);

            // Assert
            counter.Should().NotBeNull();
            counter.Should().BeOfType<CustomGauge>();
        }

        [Fact]
        public void Can_build_new_instance()
        {
            // Arrange
            double Builder() => 1.0;

            // Act
            var gauge = _builder.Build((Func<double>)Builder);

            // Assert
            gauge.Should().NotBeNull();
        }
    }
}