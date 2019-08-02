// <copyright file="DefaultHistogramManagerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.BucketHistogram;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Histogram;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultBucketHistogramManagerTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly string _context;
        private readonly MetricCoreTestFixture _fixture;
        private readonly IMeasureBucketHistogramMetrics _manager;

        public DefaultBucketHistogramManagerTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _manager = _fixture.Managers.BucketHistogram;
            _context = _fixture.Context;
        }

        [Fact]
        public void Can_update()
        {
            var metricName = "test_update_histogram";
            var options = new BucketHistogramOptions { Name = metricName };

            _manager.Update(options, 2L);

            _fixture.Snapshot.GetBucketHistogramValue(_context, metricName).Sum.Should().Be(2L);
            _fixture.Snapshot.GetBucketHistogramValue(_context, metricName).Count.Should().Be(1L);
        }

        [Fact]
        public void Can_update_multidimensional()
        {
            var metricName = "test_update_histogram_multi";
            var options = new BucketHistogramOptions { Name = metricName };

            _manager.Update(options, _fixture.Tags[0], 2L);
            _manager.Update(options, _fixture.Tags[1], 4L);

            _fixture.Snapshot.GetBucketHistogramValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Sum.Should().Be(2L);
            _fixture.Snapshot.GetBucketHistogramValue(_context, _fixture.Tags[0].AsMetricName(metricName)).Count.Should().Be(1L);
            _fixture.Snapshot.GetBucketHistogramValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Sum.Should().Be(4L);
            _fixture.Snapshot.GetBucketHistogramValue(_context, _fixture.Tags[1].AsMetricName(metricName)).Count.Should().Be(1L);
        }
    }
}