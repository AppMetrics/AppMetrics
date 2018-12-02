// <copyright file="DefaultCounterBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Counter;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.TestHelpers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.MetricBuilders
{
    public class DefaultCounterBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildCounterMetrics _builder;

        public DefaultCounterBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Counter; }

        [Fact]
        public void Can_build_custom_instance()
        {
            // Arrange
            CustomCounter Builder() => new CustomCounter();

            // Act
            var counter = _builder.Build(Builder);

            // Assert
            counter.Should().NotBeNull();
            counter.Should().BeOfType<CustomCounter>();
        }

        [Fact]
        public void Can_build_new_instance()
        {
            // Arrange
            // Act
            var counter = _builder.Build();

            // Assert
            counter.Should().NotBeNull();
        }
    }
}