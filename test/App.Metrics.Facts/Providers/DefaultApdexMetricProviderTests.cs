// <copyright file="DefaultApdexMetricProviderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultApdexMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly Func<IReservoir> _defaultReservoir = () => new DefaultAlgorithmRReservoir(1028);
        private readonly IFilterMetrics _filter = new MetricsFilter().WhereType(MetricType.Apdex);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideApdexMetrics _provider;

        public DefaultApdexMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = _fixture.Providers.Apdex;
        }

        [Fact]
        public void Can_add_add_new_instance_to_registry()
        {
            var metricName = "apdex_metric_test";
            var options = new ApdexOptions
                          {
                              Name = metricName
                          };

            var apdexMetric = _fixture.Builder.Apdex.Build(_defaultReservoir, 0.5, true, _fixture.Clock);

            _provider.Instance(options, () => apdexMetric);

            _filter.WhereName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().ApdexScores.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_add_new_multidimensional_to_registry()
        {
            var metricName = "apdex_metric_test_multi";
            var options = new ApdexOptions
                          {
                              Name = metricName
                          };

            var apdexMetric1 = _fixture.Builder.Apdex.Build(_defaultReservoir, 0.5, true, _fixture.Clock);

            _provider.Instance(options, _fixture.Tags[0], () => apdexMetric1);

            _filter.WhereName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().ApdexScores.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_instance_to_registry()
        {
            var metricName = "apdex_test";
            var options = new ApdexOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().ApdexScores.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_multidimensional_to_registry()
        {
            var metricName = "apdex_test_multi";
            var options = new ApdexOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, _fixture.Tags[0]);

            _filter.WhereName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().ApdexScores.Count().Should().Be(1);
        }

        [Fact]
        public void Can_use_custom_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var options = new ApdexOptions
                          {
                              Name = "apdex_custom_reservoir",
                              Reservoir = () => reservoirMock.Object
                          };

            var apdex = _provider.Instance(options);

            apdex.Track(100L);

            reservoirMock.Verify(r => r.Update(100L), Times.Once);
        }

        [Fact]
        public void Can_use_custom_reservoir_when_multidimensional()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var options = new ApdexOptions
                          {
                              Name = "apdex_custom_reservoir_multi",
                              Reservoir = () => reservoirMock.Object
                          };

            var apdex = _provider.Instance(options, _fixture.Tags[0]);

            apdex.Track(100L);

            reservoirMock.Verify(r => r.Update(100L), Times.Once);
        }
    }
}