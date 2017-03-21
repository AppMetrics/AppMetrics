// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Abstractions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultHistogramManagerTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly string _context;
        private readonly MetricCoreTestFixture _fixture;
        private readonly IMeasureHistogramMetrics _manager;

        public DefaultHistogramManagerTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _manager = _fixture.Managers.Histogram;
            _context = _fixture.Context;
        }

        [Fact]
        public void can_update()
        {
            var metricName = "test_update_histogram";
            var options = new HistogramOptions { Name = metricName };

            _manager.Update(options, 2L);

            _fixture.Snapshot.GetHistogramValue(_context, metricName).LastValue.Should().Be(2L);
        }

        [Fact]
        public void can_update_multidimensional()
        {
            var metricName = "test_update_histogram_multi";
            var options = new HistogramOptions { Name = metricName };

            _manager.Update(options, _fixture.Tags[0], 2L);
            _manager.Update(options, _fixture.Tags[1], 4L);

            _fixture.Snapshot.GetHistogramValue(_context, _fixture.Tags[0].AsMetricName(metricName)).LastValue.Should().Be(2L);
            _fixture.Snapshot.GetHistogramValue(_context, _fixture.Tags[1].AsMetricName(metricName)).LastValue.Should().Be(4L);
        }

        [Fact]
        public void can_update_multidimensional_with_user_value()
        {
            var metricName = "test_update_histogram_user_value_multi";
            var options = new HistogramOptions { Name = metricName };

            _manager.Update(options, _fixture.Tags[0], 5L, "uservalue");
            _manager.Update(options, _fixture.Tags[1], 100L, "uservalue");

            _fixture.Snapshot.GetHistogramValue(_context, _fixture.Tags[0].AsMetricName(metricName)).LastValue.Should().Be(5L);
            _fixture.Snapshot.GetHistogramValue(_context, _fixture.Tags[0].AsMetricName(metricName)).LastUserValue.Should().Be("uservalue");
            _fixture.Snapshot.GetHistogramValue(_context, _fixture.Tags[1].AsMetricName(metricName)).LastValue.Should().Be(100L);
            _fixture.Snapshot.GetHistogramValue(_context, _fixture.Tags[1].AsMetricName(metricName)).LastUserValue.Should().Be("uservalue");
        }

        [Fact]
        public void can_update_with_user_value()
        {
            var metricName = "test_update_histogram_user_value";
            var options = new HistogramOptions { Name = metricName };

            _manager.Update(options, 5L, "uservalue");

            _fixture.Snapshot.GetHistogramValue(_context, metricName).LastValue.Should().Be(5L);
            _fixture.Snapshot.GetHistogramValue(_context, metricName).LastUserValue.Should().Be("uservalue");
        }
    }
}