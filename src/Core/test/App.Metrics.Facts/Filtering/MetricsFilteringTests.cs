// <copyright file="MetricsFilteringTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Filtering
{
    public class MetricsFilteringTests : IClassFixture<MetricsWithSamplesFixture>
    {
        private readonly IMetrics _metrics;

        public MetricsFilteringTests(MetricsWithSamplesFixture fixture) { _metrics = fixture.Metrics; }

        [Fact]
        public void Can_filter_metrics_by_name()
        {
            var filter = new MetricsFilter().WhereName("test_gauge");
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var gaugeValue = context.Gauges.FirstOrDefault();

            gaugeValue.Should().NotBeNull();

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_name_using_predicate()
        {
            var filter = new MetricsFilter().WhereName(name => name == "test_gauge");
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var gaugeValue = context.Gauges.FirstOrDefault();

            gaugeValue.Should().NotBeNull();

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_counters()
        {
            var filter = new MetricsFilter().WhereType(MetricType.Counter);
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var counterValue = context.Counters.Single();
            counterValue.Name.Should().Be("test_counter|tag1:value1");
            counterValue.Value.Count.Should().Be(1);

            Assert.Null(context.Meters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_gauge()
        {
            var filter = new MetricsFilter().WhereType(MetricType.Gauge);
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var gaugeValue = context.Gauges.Single();
            gaugeValue.Name.Should().Be("test_gauge");
            gaugeValue.Value.Should().Be(8);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_histograms()
        {
            var filter = new MetricsFilter().WhereType(MetricType.Histogram);
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var histogramValue = context.Histograms.Single();
            histogramValue.Name.Should().Be("test_histogram");
            histogramValue.Value.LastValue.Should().Be(5);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_meters()
        {
            var filter = new MetricsFilter().WhereType(MetricType.Meter);
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var meterValue = context.Meters.Single();
            meterValue.Name.Should().Be("test_meter|tag2:value2");
            meterValue.Value.Count.Should().Be(1);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_name_starting_with()
        {
            var filter = new MetricsFilter().WhereNameStartsWith("test_");
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var counterValue = context.Counters.FirstOrDefault();
            var gaugeValue = context.Gauges.FirstOrDefault();
            var meterValue = context.Meters.FirstOrDefault();
            var histogramValue = context.Histograms.FirstOrDefault();
            var timerValue = context.Timers.FirstOrDefault();

            counterValue.Should().NotBeNull();
            gaugeValue.Should().NotBeNull();
            meterValue.Should().NotBeNull();
            histogramValue.Should().NotBeNull();
            timerValue.Should().NotBeNull();
        }

        [Fact]
        public void Can_filter_metrics_by_tags()
        {
            var filter = new MetricsFilter().WhereTaggedWithKeyValue(new TagKeyValueFilter { { "tag1", "value1" } });
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var counterValue = context.Counters.Single();

            counterValue.Tags.Keys.Should().Contain("tag1");
            counterValue.Tags.Values.Should().Contain("value1");

            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_tags_keys()
        {
            var filter = new MetricsFilter().WhereTaggedWithKey("tag1", "tag2");
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var counterValue = context.Counters.Single();
            var meterValue = context.Meters.Single();

            counterValue.Tags.Keys.Should().Contain("tag1");
            meterValue.Tags.Keys.Should().Contain("tag2");

            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Timers.FirstOrDefault());
        }

        [Fact]
        public void Can_filter_metrics_by_timers()
        {
            var filter = new MetricsFilter().WhereType(MetricType.Timer);
            var currentData = _metrics.Snapshot.Get(filter);
            var context = currentData.Contexts.Single();

            var timerValue = context.Timers.Single();
            timerValue.Name.Should().Be("test_timer");
            timerValue.Value.Histogram.Sum.Should().Be(10.0);

            Assert.Null(context.Counters.FirstOrDefault());
            Assert.Null(context.Gauges.FirstOrDefault());
            Assert.Null(context.Histograms.FirstOrDefault());
            Assert.Null(context.Meters.FirstOrDefault());
        }
    }
}