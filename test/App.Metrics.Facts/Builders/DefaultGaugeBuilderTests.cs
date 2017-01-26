// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Facts.Fixtures;
using App.Metrics.Gauge.Abstractions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultGaugeBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildGaugeMetrics _builder;

        public DefaultGaugeBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Gauge; }

        [Fact]
        public void can_build_new_instance()
        {
            var gauge = _builder.Build(() => 1.0);

            gauge.Should().NotBeNull();
        }
    }
}