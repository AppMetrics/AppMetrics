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
    public class DefaultHistogramManagerTests : IClassFixture<MetricManagerTestFixture>
    {
        private readonly MetricManagerTestFixture _fixture;
        private readonly IMeasureHistogramMetrics _manager;

        public DefaultHistogramManagerTests(MetricManagerTestFixture fixture)
        {
            _fixture = fixture;
            _manager = new DefaultHistogramManager(_fixture.Advanced, _fixture.Registry);
        }

        [Fact]
        public void can_update()
        {
            var metricName = "test_update_histogram";
            var options = new HistogramOptions { Name = metricName };

            _manager.Update(options, 2L);

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().HistogramValueFor(metricName).LastValue.Should().Be(2L);
        }

        [Fact]
        public void can_update_with_user_value()
        {
            var metricName = "test_update_histogram_user_value";
            var options = new HistogramOptions { Name = metricName };

            _manager.Update(options, 5L, "uservalue");

            var data = _fixture.Registry.GetData(new NoOpMetricsFilter());

            data.Contexts.Single().HistogramValueFor(metricName).LastValue.Should().Be(5L);
            data.Contexts.Single().HistogramValueFor(metricName).LastUserValue.Should().Be("uservalue");
        }
    }
}