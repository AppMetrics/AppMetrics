// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Facts.Fixtures;
using App.Metrics.Interfaces;
using App.Metrics.Sampling;
using App.Metrics.Sampling.Interfaces;
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
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var histogram = _builder.Build(reservoirMock.Object);

            histogram.Should().NotBeNull();
        }

        [Fact]
        public void can_build_with_sampling_params()
        {
            var histogram = _builder.Build(SamplingType.Default, 1028, 0.0015);

            histogram.Should().NotBeNull();
        }
    }
}