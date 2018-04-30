// <copyright file="DefaultApdexBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Facts.Fixtures;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.MetricBuilders
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
        public void Can_build_with_reservoir()
        {
            // Arrange
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            // Act
            var apdex = _builder.Build(() => reservoirMock.Object, 0.5, true, _fixture.Clock);

            // Assert
            apdex.Should().NotBeNull();
        }
    }
}