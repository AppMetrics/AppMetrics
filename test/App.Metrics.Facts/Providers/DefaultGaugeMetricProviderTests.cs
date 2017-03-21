// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.Gauge;
using App.Metrics.Gauge.Abstractions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultGaugeMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IFilterMetrics _filter = new DefaultMetricsFilter().WhereType(MetricType.Gauge);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideGaugeMetrics _provider;

        public DefaultGaugeMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = _fixture.Providers.Gauge;
        }

        [Fact]
        public void can_add_instance_to_registry()
        {
            var metricName = "gauge_provider_test";

            var options = new GaugeOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, () => new FunctionGauge(() => 1.0));

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Gauges.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_instance_to_registry_with_default_builder()
        {
            var metricName = "gauge_provider_test_default_builder";

            var options = new GaugeOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Gauges.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_multidimensional_to_registry()
        {
            var metricName = "gauge_provider_test_multi";

            var options = new GaugeOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, _fixture.Tags[0], () => new FunctionGauge(() => 1.0));

            _filter.WhereMetricName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Gauges.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_multidimensional_to_registry_with_default_builder()
        {
            var metricName = "gauge_provider_test_default_builder";

            var options = new GaugeOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, _fixture.Tags[0]);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Gauges.Count().Should().Be(1);
        }

        [Fact]
        public void same_metric_only_added_once()
        {
            var metricName = "gauge_provider_test";

            var options = new GaugeOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, () => new FunctionGauge(() => 1.0));
            _provider.Instance(options, () => new FunctionGauge(() => 2.0));

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Gauges.Count().Should().Be(1);
        }

        [Fact]
        public void same_metric_only_added_once_when_multidimensional()
        {
            var metricName = "gauge_provider_test_multi";

            var options = new GaugeOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, _fixture.Tags[0], () => new FunctionGauge(() => 1.0));
            _provider.Instance(options, _fixture.Tags[0], () => new FunctionGauge(() => 2.0));

            _filter.WhereMetricName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Gauges.Count().Should().Be(1);
        }
    }
}