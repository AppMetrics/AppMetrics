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
            var options = new GaugeOptions() { Name = metricName };

            _manager.SetValue(options, () => 2.0);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().Gauges.Single(g => g.Name == metricName).Value.Should().Be(2.0);
        }
    }
}