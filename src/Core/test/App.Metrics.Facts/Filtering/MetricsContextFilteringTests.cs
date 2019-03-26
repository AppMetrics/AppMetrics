// <copyright file="MetricsContextFilteringTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Filtering
{
    public class MetricsContextFilteringTests : IClassFixture<MetricsWithMultipleContextsSamplesFixture>
    {
        private readonly IMetrics _metrics;

        public MetricsContextFilteringTests(MetricsWithMultipleContextsSamplesFixture fixture) { _metrics = fixture.Metrics; }

        [Fact]
        public void Can_filter_metrics_by_context()
        {
            var filter = new MetricsFilter().WhereContext("test_context1");
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var counterValue = context.Counters.Single();
            counterValue.Name.Should().Be("test_counter|tag1:value");
            counterValue.Value.Count.Should().Be(1);

            Assert.Null(context.Meters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_context_via_data_provider()
        {
            var currentData = _metrics.Snapshot.GetForContext("test_context1");

            var counterValue = currentData.Counters.Single();
            counterValue.Name.Should().Be("test_counter|tag1:value");
            counterValue.Value.Count.Should().Be(1);

            Assert.Null(currentData.Meters.FirstOrDefault());
            Assert.Null(currentData.Gauges.FirstOrDefault());
            Assert.Null(currentData.Histograms.FirstOrDefault());
            Assert.Null(currentData.Timers.FirstOrDefault());
        }
    }
}