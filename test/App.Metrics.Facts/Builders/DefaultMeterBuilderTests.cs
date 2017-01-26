// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Facts.Fixtures;
using App.Metrics.Meter.Abstractions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
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
        public void can_mark()
        {
            var meter = _builder.Build(_fixture.Clock);

            meter.Should().NotBeNull();
        }
    }
}