// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultHistogramMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IFilterMetrics _filter = new DefaultMetricsFilter().WhereType(MetricType.Histogram);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideHistogramMetrics _provider;


        public DefaultHistogramMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = _fixture.Providers.Histogram;
        }

        [Fact]
        public void can_add_add_new_instance_to_registry()
        {
            var metricName = "histogram_provider_metric_test";
            var options = new HistogramOptions
                          {
                              Name = metricName
                          };

            var reservoir = new Lazy<IReservoir>(() => new DefaultAlgorithmRReservoir(1028));

            var apdexMetric = _fixture.Builder.Histogram.Build(reservoir);

            _provider.Instance(options, () => apdexMetric);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Histograms.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_instance_to_registry()
        {
            var metricName = "histogram_provider_test";
            var options = new HistogramOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Histograms.Count().Should().Be(1);
        }


        [Fact]
        public void can_use_custom_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(100L));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100L, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var reservoir = new Lazy<IReservoir>(() => reservoirMock.Object);

            var options = new HistogramOptions
                          {
                              Name = "histogram_provider_custom_test",
                              Reservoir = reservoir
                          };

            var histogram = _provider.Instance(options);

            histogram.Update(100L);

            reservoirMock.Verify(r => r.Update(100L), Times.Once);
        }
    }
}