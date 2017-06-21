// <copyright file="DefaultCounterBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Counter;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.TestHelpers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultCounterBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildCounterMetrics _builder;

        public DefaultCounterBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Counter; }

        [Fact]
        public void Can_build_custom_instance()
        {
            var counter = _builder.Build(() => new CustomCounter());

            counter.Should().NotBeNull();
            counter.Should().BeOfType<CustomCounter>();
        }

        [Fact]
        public void Can_build_new_instance()
        {
            var counter = _builder.Build();

            counter.Should().NotBeNull();
        }
    }
}