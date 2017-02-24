// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Core.Options;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultCounterMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IFilterMetrics _filter = new DefaultMetricsFilter().WhereType(MetricType.Counter);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideCounterMetrics _provider;

        public DefaultCounterMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = fixture.Providers.Counter;
        }

        [Fact]
        public void can_add_add_new_instance_to_registry()
        {
            var metricName = "counter_metric_provider_test";
            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            var counterMetric = _fixture.Builder.Counter.Build();

            _provider.Instance(options, () => counterMetric);

            _filter.WhereMetricName(name => name == "counter_metric_provider_test");

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_add_new_multidimensional_to_registry()
        {
            var metricName = "counter_metric_provider_test_multi";
            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            var counterMetric = _fixture.Builder.Counter.Build();

            _provider.Instance(options, _fixture.Tags[0], () => counterMetric);

            _filter.WhereMetricName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_instance_to_registry()
        {
            var metricName = "counter_provider_test";

            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }

        [Fact]
        public void can_add_multidimensional_to_registry()
        {
            var metricName = "counter_provider_test_multi";

            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, _fixture.Tags[0]);

            _filter.WhereMetricName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }
    }
}