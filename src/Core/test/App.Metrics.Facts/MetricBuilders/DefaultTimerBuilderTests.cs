// <copyright file="DefaultTimerBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Facts.Fixtures;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Timer;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.MetricBuilders
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
        public void Can_build_with_histogram()
        {
            // Arrange
            var histogram = new Mock<IHistogramMetric>();
            histogram.Setup(r => r.Update(It.IsAny<long>(), null));
            histogram.Setup(r => r.Reset());

            // Act
            var timer = _builder.Build(histogram.Object, _fixture.Clock);

            // Assert
            timer.Should().NotBeNull();
        }

        [Fact]
        public void Can_build_with_histogram_and_meter()
        {
            // Arrange
            var histogramMock = new Mock<IHistogramMetric>();
            var meterMock = new Mock<IMeterMetric>();
            histogramMock.Setup(r => r.Update(It.IsAny<long>(), null));
            histogramMock.Setup(r => r.Reset());
            meterMock.Setup(r => r.GetValue(false));

            // Act
            var timer = _builder.Build(histogramMock.Object, meterMock.Object, _fixture.Clock);

            // Assert
            timer.Should().NotBeNull();
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
            var timer = _builder.Build(() => reservoirMock.Object, _fixture.Clock);

            // Assert
            timer.Should().NotBeNull();
        }

        [Fact]
        public void Can_build_with_reservoir_and_meter()
        {
            // Arrange
            var reservoirMock = new Mock<IReservoir>();
            var meterMock = new Mock<IMeterMetric>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());
            meterMock.Setup(r => r.GetValue(false));

            // Act
            var timer = _builder.Build(() => reservoirMock.Object, meterMock.Object, _fixture.Clock);

            // Assert
            timer.Should().NotBeNull();
        }
    }
}