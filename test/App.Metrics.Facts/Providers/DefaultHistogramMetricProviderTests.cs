// <copyright file="DefaultHistogramMetricProviderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Histogram;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultHistogramMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IFilterMetrics _filter = new MetricsFilter().WhereType(MetricType.Histogram);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideHistogramMetrics _provider;

        public DefaultHistogramMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = _fixture.Providers.Histogram;
        }

        [Fact]
        public void Can_add_add_new_instance_to_registry()
        {
            var metricName = "histogram_provider_metric_test";
            var options = new HistogramOptions
                          {
                              Name = metricName
                          };

            var apdexMetric = _fixture.Builder.Histogram.Build(() => new DefaultAlgorithmRReservoir(1028));

            _provider.Instance(options, () => apdexMetric);

            _filter.WhereName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Histograms.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_add_new_multidimensional_to_registry()
        {
            var metricName = "histogram_provider_metric_test_multi";
            var options = new HistogramOptions
                          {
                              Name = metricName
                          };

            var apdexMetric = _fixture.Builder.Histogram.Build(() => new DefaultAlgorithmRReservoir(1028));

            _provider.Instance(options, _fixture.Tags[0], () => apdexMetric);

            _filter.WhereName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Histograms.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_instance_to_registry()
        {
            var metricName = "histogram_provider_test";
            var options = new HistogramOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Histograms.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_multidimensional_to_registry()
        {
            var metricName = "histogram_provider_test_multi";
            var options = new HistogramOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, _fixture.Tags[0]);

            _filter.WhereName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Histograms.Count().Should().Be(1);
        }

        [Fact]
        public void Can_use_custom_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(100L));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100L, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var options = new HistogramOptions
                          {
                              Name = "histogram_provider_custom_test",
                              Reservoir = () => reservoirMock.Object
                          };

            var histogram = _provider.Instance(options);

            histogram.Update(100L);

            reservoirMock.Verify(r => r.Update(100L), Times.Once);
        }

        [Fact]
        public void Can_use_custom_reservoir_when_multidimensional()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(100L));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100L, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var options = new HistogramOptions
                          {
                              Name = "histogram_provider_custom_test_multi",
                              Reservoir = () => reservoirMock.Object
                          };

            var histogram = _provider.Instance(options, _fixture.Tags[0]);

            histogram.Update(100L);

            reservoirMock.Verify(r => r.Update(100L), Times.Once);
        }
    }
}