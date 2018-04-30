// <copyright file="DefaultHistogramBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Facts.Fixtures;
using App.Metrics.Histogram;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.MetricBuilders
{
    public class DefaultHistogramBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildHistogramMetrics _builder;

        public DefaultHistogramBuilderTests(MetricCoreTestFixture fixture) { _builder = fixture.Builder.Histogram; }

        [Fact]
        public void Can_build_with_reservoir()
        {
            // Arrange
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            // Act
            var histogram = _builder.Build(() => reservoirMock.Object);

            // Assert
            histogram.Should().NotBeNull();
        }
    }
}