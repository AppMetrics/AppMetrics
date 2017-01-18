// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Internal.Managers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultGaugeManagerTests : IClassFixture<MetricManagerTestFixture>
    {
        private readonly MetricManagerTestFixture _fixture;
        private readonly IMeasureGaugeMetrics _manager;

        public DefaultGaugeManagerTests(MetricManagerTestFixture fixture)
        {
            _fixture = fixture;
            _manager = new DefaultGaugeManager(_fixture.Builder.Gauge, _fixture.Registry);
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