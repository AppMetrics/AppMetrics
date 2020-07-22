// <copyright file="MetricSnapshotHostedMetricsJsonWriterTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Serialization;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.GrafanaCloudHostedMetrics.Facts
{
    public class MetricSnapshotHostedMetricsJsonWriterTests
    {
        private const string MultidimensionalMetricNameSuffix = "|host:server1,env:staging";
        private static readonly TimeSpan FlushInterval = TimeSpan.FromSeconds(10);
        private readonly IReservoir _defaultReservoir = new DefaultForwardDecayingReservoir();
        private readonly MetricTags _tags = new MetricTags(new[] { "host", "env" }, new[] { "server1", "staging" });
        private readonly DateTime _timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);

        [Fact]
        public async Task Can_report_apdex()
        {
            // Arrange
            var expected =
                "[{\"name\":\"apdex.test.test_apdex.samples\",\"metric\":\"apdex.test.test_apdex.samples\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.score\",\"metric\":\"apdex.test.test_apdex.score\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.satisfied\",\"metric\":\"apdex.test.test_apdex.satisfied\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.tolerating\",\"metric\":\"apdex.test.test_apdex.tolerating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.frustrating\",\"metric\":\"apdex.test.test_apdex.frustrating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex",
                ConstantValue.Provider(apdex.Value),
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_apdex__when_multidimensional()
        {
            // Arrange
            var expected =
                "[{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.samples\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.samples\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.score\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.score\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.satisfied\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.satisfied\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.tolerating\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.tolerating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.frustrating\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.frustrating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(apdex.Value),
                _tags);

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_apdex_with_tags()
        {
            // Arrange
            var expected =
                "[{\"name\":\"apdex.test.test_apdex.key1.value1.key2.value2.samples\",\"metric\":\"apdex.test.test_apdex.key1.value1.key2.value2.samples\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.key1.value1.key2.value2.score\",\"metric\":\"apdex.test.test_apdex.key1.value1.key2.value2.score\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.key1.value1.key2.value2.satisfied\",\"metric\":\"apdex.test.test_apdex.key1.value1.key2.value2.satisfied\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.key1.value1.key2.value2.tolerating\",\"metric\":\"apdex.test.test_apdex.key1.value1.key2.value2.tolerating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"apdex.test.test_apdex.key1.value1.key2.value2.frustrating\",\"metric\":\"apdex.test.test_apdex.key1.value1.key2.value2.frustrating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex",
                ConstantValue.Provider(apdex.Value),
                new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" }));

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_apdex_with_tags_when_multidimensional()
        {
            // Arrange
            var expected =
                "[{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.samples\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.samples\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.score\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.score\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.satisfied\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.satisfied\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.tolerating\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.tolerating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.frustrating\",\"metric\":\"env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.frustrating\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(apdex.Value),
                MetricTags.Concat(_tags, new MetricTags("anothertag", "thevalue")));

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items()
        {
            // Arrange
            var expected =
                "[{\"name\":\"counter.test.test_counter_items.item.item1_value1.total\",\"metric\":\"counter.test.test_counter_items.item.item1_value1.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter_items.item.item1_value1.percent\",\"metric\":\"counter.test.test_counter_items.item.item1_value1.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter_items.item.item2_value2.total\",\"metric\":\"counter.test.test_counter_items.item.item2_value2.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter_items.item.item2_value2.percent\",\"metric\":\"counter.test.test_counter_items.item.item2_value2.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter.value\",\"metric\":\"counter.test.test_counter.value\",\"value\":2,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items_and_tags()
        {
            // Arrange
            var expected =
                "[{\"name\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item1_value1.total\",\"metric\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item1_value1.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item1_value1.percent\",\"metric\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item1_value1.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item2_value2.total\",\"metric\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item2_value2.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item2_value2.percent\",\"metric\":\"counter.test.test_counter_items.key1.value1.key2.value2.item.item2_value2.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter.key1.value1.key2.value2.value\",\"metric\":\"counter.test.test_counter.key1.value1.key2.value2.value\",\"value\":2,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" }));

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items_tags_when_multidimensional()
        {
            // Arrange
            var expected =
                "[{\"name\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item1_value1.total\",\"metric\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item1_value1.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item1_value1.percent\",\"metric\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item1_value1.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item2_value2.total\",\"metric\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item2_value2.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item2_value2.percent\",\"metric\":\"env.staging.counter.test.test_counter_items.host.server1.key1.value1.key2.value2.item.item2_value2.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.counter.test.test_counter.host.server1.key1.value1.key2.value2.value\",\"metric\":\"env.staging.counter.test.test_counter.host.server1.key1.value1.key2.value2.value\",\"value\":2,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var counterTags = new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" });
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Concat(_tags, counterTags));

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items_with_option_not_to_report_percentage()
        {
            // Arrange
            var expected =
                "[{\"name\":\"counter.test.test_counter_items.item.item1_value1.total\",\"metric\":\"counter.test.test_counter_items.item.item1_value1.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter_items.item.item2_value2.total\",\"metric\":\"counter.test.test_counter_items.item.item2_value2.total\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"counter.test.test_counter.value\",\"metric\":\"counter.test.test_counter.value\",\"value\":2,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty,
                reportItemPercentages: false);

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task  Can_report_counters()
        {
            // Arrange
            var expected = "[{\"name\":\"counter.test.test_counter.value\",\"metric\":\"counter.test.test_counter.value\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var counter = new DefaultCounterMetric();
            counter.Increment(1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_counters__when_multidimensional()
        {
            // Arrange
            var expected = "[{\"name\":\"env.staging.counter.test.test_counter.host.server1.value\",\"metric\":\"env.staging.counter.test.test_counter.host.server1.value\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var counter = new DefaultCounterMetric();
            counter.Increment(1);
            var counterValueSource = new CounterValueSource(
                "test counter" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(counter.Value),
                Unit.None,
                _tags);

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_gauges()
        {
            // Arrange
            var expected = "[{\"name\":\"gauge.test.test_gauge.value\",\"metric\":\"gauge.test.test_gauge.value\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource(
                "test gauge",
                ConstantValue.Provider(gauge.Value),
                Unit.None,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", gauges: gaugeValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_gauges__when_multidimensional()
        {
            // Arrange
            var expected = "[{\"name\":\"env.staging.gauge.test.gauge-group.host.server1.value\",\"metric\":\"env.staging.gauge.test.gauge-group.host.server1.value\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource(
                "gauge-group" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(gauge.Value),
                Unit.None,
                _tags);

            // Act
            var valueSource = CreateValueSource("test", gauges: gaugeValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_histograms()
        {
            // Arrange
            var expected =
                "[{\"name\":\"histogram.test.test_histogram.samples\",\"metric\":\"histogram.test.test_histogram.samples\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.last\",\"metric\":\"histogram.test.test_histogram.last\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.count_hist\",\"metric\":\"histogram.test.test_histogram.count_hist\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.sum\",\"metric\":\"histogram.test.test_histogram.sum\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.min\",\"metric\":\"histogram.test.test_histogram.min\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.max\",\"metric\":\"histogram.test.test_histogram.max\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.mean\",\"metric\":\"histogram.test.test_histogram.mean\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.median\",\"metric\":\"histogram.test.test_histogram.median\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.stddev\",\"metric\":\"histogram.test.test_histogram.stddev\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.p999\",\"metric\":\"histogram.test.test_histogram.p999\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.p99\",\"metric\":\"histogram.test.test_histogram.p99\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.p98\",\"metric\":\"histogram.test.test_histogram.p98\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.p95\",\"metric\":\"histogram.test.test_histogram.p95\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.p75\",\"metric\":\"histogram.test.test_histogram.p75\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.user_last\",\"metric\":\"histogram.test.test_histogram.user_last\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.user_min\",\"metric\":\"histogram.test.test_histogram.user_min\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"histogram.test.test_histogram.user_max\",\"metric\":\"histogram.test.test_histogram.user_max\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var histogram = new DefaultHistogramMetric(_defaultReservoir);
            histogram.Update(1000, "client1");
            var histogramValueSource = new HistogramValueSource(
                "test histogram",
                ConstantValue.Provider(histogram.Value),
                Unit.None,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", histograms: histogramValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_histograms_when_multidimensional()
        {
            // Arrange
            var expected =
                "[{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.samples\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.samples\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.last\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.last\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.count_hist\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.count_hist\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.sum\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.sum\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.min\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.min\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.max\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.max\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.mean\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.mean\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.median\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.median\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.stddev\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.stddev\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.p999\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.p999\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.p99\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.p99\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.p98\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.p98\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.p95\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.p95\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.p75\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.p75\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.user_last\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.user_last\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.user_min\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.user_min\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.histogram.test.test_histogram.host.server1.user_max\",\"metric\":\"env.staging.histogram.test.test_histogram.host.server1.user_max\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var histogram = new DefaultHistogramMetric(_defaultReservoir);
            histogram.Update(1000, "client1");
            var histogramValueSource = new HistogramValueSource(
                "test histogram" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(histogram.Value),
                Unit.None,
                _tags);

            // Act
            var valueSource = CreateValueSource("test", histograms: histogramValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_meters()
        {
            // Arrange
            var expected = "[{\"name\":\"meter.test.test_meter.count_meter\",\"metric\":\"meter.test.test_meter.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter.rate1m\",\"metric\":\"meter.test.test_meter.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter.rate5m\",\"metric\":\"meter.test.test_meter.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter.rate15m\",\"metric\":\"meter.test.test_meter.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            meter.Mark(1);
            var meterValueSource = new MeterValueSource(
                "test meter",
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", meters: meterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_meters_when_multidimensional()
        {
            // Arrange
            var expected =
                "[{\"name\":\"env.staging.meter.test.test_meter.host.server1.count_meter\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter.host.server1.rate1m\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter.host.server1.rate5m\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter.host.server1.rate15m\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            meter.Mark(1);
            var meterValueSource = new MeterValueSource(
                "test meter" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                _tags);

            // Act
            var valueSource = CreateValueSource("test", meters: meterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_meters_with_items()
        {
            // Arrange
            var expected =
                "[{\"name\":\"meter.test.test_meter_items.item.item1_value1.count_meter\",\"metric\":\"meter.test.test_meter_items.item.item1_value1.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item1_value1.rate1m\",\"metric\":\"meter.test.test_meter_items.item.item1_value1.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item1_value1.rate5m\",\"metric\":\"meter.test.test_meter_items.item.item1_value1.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item1_value1.rate15m\",\"metric\":\"meter.test.test_meter_items.item.item1_value1.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item1_value1.percent\",\"metric\":\"meter.test.test_meter_items.item.item1_value1.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item2_value2.count_meter\",\"metric\":\"meter.test.test_meter_items.item.item2_value2.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item2_value2.rate1m\",\"metric\":\"meter.test.test_meter_items.item.item2_value2.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item2_value2.rate5m\",\"metric\":\"meter.test.test_meter_items.item.item2_value2.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item2_value2.rate15m\",\"metric\":\"meter.test.test_meter_items.item.item2_value2.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter_items.item.item2_value2.percent\",\"metric\":\"meter.test.test_meter_items.item.item2_value2.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter.count_meter\",\"metric\":\"meter.test.test_meter.count_meter\",\"value\":2,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter.rate1m\",\"metric\":\"meter.test.test_meter.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter.rate5m\",\"metric\":\"meter.test.test_meter.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"meter.test.test_meter.rate15m\",\"metric\":\"meter.test.test_meter.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            meter.Mark(new MetricSetItem("item1", "value1"), 1);
            meter.Mark(new MetricSetItem("item2", "value2"), 1);
            var meterValueSource = new MeterValueSource(
                "test meter",
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", meters: meterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_meters_with_items_tags_when_multidimensional()
        {
            // Arrange
            var expected =
                "[{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.count_meter\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.rate1m\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.rate5m\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.rate15m\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.percent\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item1_value1.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.count_meter\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.rate1m\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.rate5m\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.rate15m\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.percent\",\"metric\":\"env.staging.meter.test.test_meter_items.host.server1.item.item2_value2.percent\",\"value\":50,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter.host.server1.count_meter\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.count_meter\",\"value\":2,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter.host.server1.rate1m\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter.host.server1.rate5m\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.meter.test.test_meter.host.server1.rate15m\",\"metric\":\"env.staging.meter.test.test_meter.host.server1.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            meter.Mark(new MetricSetItem("item1", "value1"), 1);
            meter.Mark(new MetricSetItem("item2", "value2"), 1);
            var meterValueSource = new MeterValueSource(
                "test meter" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                _tags);

            // Act
            var valueSource = CreateValueSource("test", meters: meterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_timers()
        {
            // Arrange
            var expected =
                "[{\"name\":\"timer.test.test_timer.count_meter\",\"metric\":\"timer.test.test_timer.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.rate1m\",\"metric\":\"timer.test.test_timer.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.rate5m\",\"metric\":\"timer.test.test_timer.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.rate15m\",\"metric\":\"timer.test.test_timer.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.samples\",\"metric\":\"timer.test.test_timer.samples\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.last\",\"metric\":\"timer.test.test_timer.last\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.count_hist\",\"metric\":\"timer.test.test_timer.count_hist\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.sum\",\"metric\":\"timer.test.test_timer.sum\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.min\",\"metric\":\"timer.test.test_timer.min\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.max\",\"metric\":\"timer.test.test_timer.max\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.mean\",\"metric\":\"timer.test.test_timer.mean\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.median\",\"metric\":\"timer.test.test_timer.median\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.stddev\",\"metric\":\"timer.test.test_timer.stddev\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.p999\",\"metric\":\"timer.test.test_timer.p999\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.p99\",\"metric\":\"timer.test.test_timer.p99\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.p98\",\"metric\":\"timer.test.test_timer.p98\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.p95\",\"metric\":\"timer.test.test_timer.p95\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.p75\",\"metric\":\"timer.test.test_timer.p75\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.user_last\",\"metric\":\"timer.test.test_timer.user_last\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.user_min\",\"metric\":\"timer.test.test_timer.user_min\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"timer.test.test_timer.user_max\",\"metric\":\"timer.test.test_timer.user_max\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var timer = new DefaultTimerMetric(_defaultReservoir, clock);
            timer.Record(1000, TimeUnit.Milliseconds, "client1");
            var timerValueSource = new TimerValueSource(
                "test timer",
                ConstantValue.Provider(timer.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                TimeUnit.Milliseconds,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", timers: timerValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        [Fact]
        public async Task Can_report_timers__when_multidimensional()
        {
            // Arrange
            var expected =
                "[{\"name\":\"env.staging.timer.test.test_timer.host.server1.count_meter\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.count_meter\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.rate1m\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.rate1m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.rate5m\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.rate5m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.rate15m\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.rate15m\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.samples\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.samples\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.last\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.last\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.count_hist\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.count_hist\",\"value\":1,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.sum\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.sum\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.min\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.min\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.max\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.max\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.mean\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.mean\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.median\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.median\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.stddev\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.stddev\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.p999\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.p999\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.p99\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.p99\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.p98\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.p98\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.p95\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.p95\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.p75\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.p75\",\"value\":1000,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.user_last\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.user_last\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.user_min\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.user_min\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"env.staging.timer.test.test_timer.host.server1.user_max\",\"metric\":\"env.staging.timer.test.test_timer.host.server1.user_max\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]";
            var clock = new TestClock();
            var timer = new DefaultTimerMetric(_defaultReservoir, clock);
            timer.Record(1000, TimeUnit.Milliseconds, "client1");
            var timerValueSource = new TimerValueSource(
                "test timer" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(timer.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                TimeUnit.Milliseconds,
                _tags);

            // Act
            var valueSource = CreateValueSource("test", timers: timerValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), FlushInterval, expected);
        }

        private async Task AssertExpectedLineProtocolString(MetricsDataValueSource dataValueSource, TimeSpan flushInterval, string expected)
        {
            var settings = new MetricsHostedMetricsOptions();
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();

            await using var ms = new MemoryStream();
            await using (var packer = new MetricSnapshotHostedMetricsJsonWriter(ms, flushInterval, settings.MetricNameFormatter))
            {
                serializer.Serialize(packer, dataValueSource, fields);
            }

            Encoding.UTF8.GetString(ms.ToArray()).Should().Be(expected);
        }

        private MetricsContextValueSource CreateValueSource(
            string context,
            GaugeValueSource gauges = null,
            CounterValueSource counters = null,
            MeterValueSource meters = null,
            HistogramValueSource histograms = null,
            BucketHistogramValueSource bucketHistograms = null,
            TimerValueSource timers = null,
            BucketTimerValueSource bucketTimers = null,
            ApdexValueSource apdexScores = null)
        {
            var gaugeValues = gauges != null ? new[] { gauges } : Enumerable.Empty<GaugeValueSource>();
            var counterValues = counters != null ? new[] { counters } : Enumerable.Empty<CounterValueSource>();
            var meterValues = meters != null ? new[] { meters } : Enumerable.Empty<MeterValueSource>();
            var histogramValues = histograms != null ? new[] { histograms } : Enumerable.Empty<HistogramValueSource>();
            var bucketHistogramValues = bucketHistograms != null ? new[] { bucketHistograms } : Enumerable.Empty<BucketHistogramValueSource>();
            var timerValues = timers != null ? new[] { timers } : Enumerable.Empty<TimerValueSource>();
            var bucketTimerValues = bucketTimers != null ? new[] { bucketTimers } : Enumerable.Empty<BucketTimerValueSource>();
            var apdexScoreValues = apdexScores != null ? new[] { apdexScores } : Enumerable.Empty<ApdexValueSource>();

            return new MetricsContextValueSource(context, gaugeValues, counterValues, meterValues, histogramValues, bucketHistogramValues, timerValues, bucketTimerValues, apdexScoreValues);
        }
    }
}