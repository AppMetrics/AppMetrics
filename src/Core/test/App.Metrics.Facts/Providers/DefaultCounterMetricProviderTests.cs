// <copyright file="DefaultCounterMetricProviderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Counter;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultCounterMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IFilterMetrics _filter = new MetricsFilter().WhereType(MetricType.Counter);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideCounterMetrics _provider;

        public DefaultCounterMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provider = fixture.Providers.Counter;
        }

        [Fact]
        public void Can_add_add_new_instance_to_registry()
        {
            var metricName = "counter_metric_provider_test";
            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            var counterMetric = _fixture.Builder.Counter.Build();

            _provider.Instance(options, () => counterMetric);

            _filter.WhereName(name => name == "counter_metric_provider_test");

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_add_new_multidimensional_to_registry()
        {
            var metricName = "counter_metric_provider_test_multi";
            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            var counterMetric = _fixture.Builder.Counter.Build();

            _provider.Instance(options, _fixture.Tags[0], () => counterMetric);

            _filter.WhereName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_instance_to_registry()
        {
            var metricName = "counter_provider_test";

            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options);

            _filter.WhereName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_multidimensional_to_registry()
        {
            var metricName = "counter_provider_test_multi";

            var options = new CounterOptions
                          {
                              Name = metricName
                          };

            _provider.Instance(options, _fixture.Tags[0]);

            _filter.WhereName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Counters.Count().Should().Be(1);
        }
    }
}