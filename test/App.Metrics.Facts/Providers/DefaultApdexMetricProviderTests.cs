// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultApdexMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly Lazy<IReservoir> _defaultReservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir(1028));
        private readonly IFilterMetrics _filter = new DefaultMetricsFilter().WhereType(MetricType.Apdex);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideApdexMetrics _provider;

        public DefaultApdexMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = _fixture.Providers.Apdex;
        }

        [Fact]
        public void can_add_add_new_instance_to_registry()
        {
            var metricName = "apdex_metric_test";
            var options = new ApdexOptions
                          {
                              Name = metricName
                          };

            var apdexMetric = _fixture.Builder.Apdex.Build(_defaultReservoir, 0.5, true, _fixture.Clock);

            _provider.Instance(options, () => apdexMetric);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().ApdexScores.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_instance_to_registry()
        {
            var metricName = "apdex_test";
            var options = new ApdexOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().ApdexScores.Count().Should().Be(1);
        }

        [Fact]
        public void can_use_custom_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var options = new ApdexOptions
                          {
                              Name = "apdex_custom_reservoir",
                              Reservoir = new Lazy<IReservoir>(() => reservoirMock.Object)
                          };

            var apdex = _provider.Instance(options);

            apdex.Track(100L);

            reservoirMock.Verify(r => r.Update(100L), Times.Once);
        }
    }
}