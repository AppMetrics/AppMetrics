// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Counter.Abstractions;
using App.Metrics.Facts.Fixtures;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultCounterBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildCounterMetrics _builder;

        public DefaultCounterBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Counter; }

        [Fact]
        public void can_build_custom_instance()
        {
            var counter = _builder.Build(() => new CustomCounter());

            counter.Should().NotBeNull();
            counter.Should().BeOfType<CustomCounter>();
        }

        [Fact]
        public void can_build_new_instance()
        {
            var counter = _builder.Build();

            counter.Should().NotBeNull();
        }
    }
}