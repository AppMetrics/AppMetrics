// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultHistogramBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildHistogramMetrics _builder;

        public DefaultHistogramBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Histogram; }

        [Fact]
        public void can_build_with_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var reservoir = new Lazy<IReservoir>(() => reservoirMock.Object);

            var histogram = _builder.Build(reservoir);

            histogram.Should().NotBeNull();
        }
    }
}