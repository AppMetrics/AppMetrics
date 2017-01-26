// // Copyright (c) Allan Hardy. All rights reserved.
// // Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Facts.Fixtures;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultApdexBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildApdexMetrics _builder;
        private readonly MetricCoreTestFixture _fixture;

        public DefaultApdexBuilderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _builder = _fixture.Builder.Apdex;
        }

        [Fact]
        public void can_build_with_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var reservoir = new Lazy<IReservoir>(() => reservoirMock.Object);

            var apdex = _builder.Build(reservoir, 0.5, true, _fixture.Clock);

            apdex.Should().NotBeNull();
        }
    }
}