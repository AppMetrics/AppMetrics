// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Gauge.Abstractions;
using App.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultGaugeManagerTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly MetricCoreTestFixture _fixture;
        private readonly IMeasureGaugeMetrics _manager;

        public DefaultGaugeManagerTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _manager = _fixture.Managers.Gauge;
        }

        [Fact]
        public void can_set_value()
        {
            var metricName = "test_set_gauge";
            var options = new GaugeOptions { Name = metricName };

            _manager.SetValue(options, () => 2.0);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().Gauges.Single(g => g.Name == metricName).Value.Should().Be(2.0);
        }

        [Fact]
        public void can_set_value_gauge()
        {
            var metricName = "test_set_value_gauge";
            var options = new GaugeOptions { Name = metricName };
            
            _manager.SetValue(options, 2.0);
            _manager.SetValue(options, 3.0);
            _manager.SetValue(options, 4.0);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().Gauges.Single(g => g.Name == metricName).Value.Should().Be(4.0);
        }

        [Fact]
        public void can_set_value_when_multidimensional()
        {
            var metricName = "test_set_gauge_multi";
            var options = new GaugeOptions { Name = metricName };

            _manager.SetValue(options, _fixture.Tags[0], () => 2.0);
            _manager.SetValue(options, _fixture.Tags[1], () => 4.0);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().Gauges.Single(g => g.Name == _fixture.Tags[0].AsMetricName(metricName)).Value.Should().Be(2.0);
            data.Contexts.Single().Gauges.Single(g => g.Name == _fixture.Tags[1].AsMetricName(metricName)).Value.Should().Be(4.0);
        }

        [Fact]
        public void can_set_value_gauge_when_multidimensional()
        {
            var metricName = "test_set_value_gauge_multi";
            var options = new GaugeOptions { Name = metricName };

            _manager.SetValue(options, _fixture.Tags[0], 2.0);
            _manager.SetValue(options, _fixture.Tags[1], 4.0);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().Gauges.Single(g => g.Name == _fixture.Tags[0].AsMetricName(metricName)).Value.Should().Be(2.0);
            data.Contexts.Single().Gauges.Single(g => g.Name == _fixture.Tags[1].AsMetricName(metricName)).Value.Should().Be(4.0);
        }
    }
}