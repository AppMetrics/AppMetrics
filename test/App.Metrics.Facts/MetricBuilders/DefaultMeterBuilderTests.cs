// <copyright file="DefaultMeterBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Facts.Fixtures;
using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.MetricBuilders
{
    public class DefaultMeterBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildMeterMetrics _builder;
        private readonly MetricCoreTestFixture _fixture;

        public DefaultMeterBuilderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _builder = _fixture.Builder.Meter;
        }

        [Fact]
        public void Can_mark()
        {
            // Arrange
            // Act
            var meter = _builder.Build(_fixture.Clock);

            // Assert
            meter.Should().NotBeNull();
        }
    }
}