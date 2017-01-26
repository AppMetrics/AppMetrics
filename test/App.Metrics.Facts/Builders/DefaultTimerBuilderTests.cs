// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Facts.Fixtures;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Timer.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultTimerBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildTimerMetrics _builder;
        private readonly MetricCoreTestFixture _fixture;

        public DefaultTimerBuilderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _builder = _fixture.Builder.Timer;
        }

        [Fact]
        public void can_build_with_histogram()
        {
            var histogram = new Mock<IHistogramMetric>();
            histogram.Setup(r => r.Update(It.IsAny<long>(), null));
            histogram.Setup(r => r.Reset());

            var timer = _builder.Build(histogram.Object, _fixture.Clock);

            timer.Should().NotBeNull();
        }

        [Fact]
        public void can_build_with_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var reservoir = new Lazy<IReservoir>(() => reservoirMock.Object);

            var timer = _builder.Build(reservoir, _fixture.Clock);

            timer.Should().NotBeNull();
        }
    }
}