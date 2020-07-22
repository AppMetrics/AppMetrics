// <copyright file="MetricSnapshotDatadogJsonWriterTests.cs" company="App Metrics Contributors">
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
using App.Metrics.Formatting.Datadog;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Serialization;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Datadog.Facts
{
    public class MetricSnapshotDatadogJsonWriterTests
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
                "{\"series\":[{\"metric\":\"test.test_apdex.apdex.samples\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.score\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.satisfied\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.tolerating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.frustrating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:result\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_apdex.apdex.samples\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.score\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.satisfied\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.tolerating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.frustrating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:result\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_apdex.apdex.samples\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"key1:value1\",\"key2:value2\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.score\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"key1:value1\",\"key2:value2\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.satisfied\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"key1:value1\",\"key2:value2\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.tolerating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"key1:value1\",\"key2:value2\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.frustrating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"key1:value1\",\"key2:value2\",\"unit:result\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_apdex.apdex.samples\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"anothertag:thevalue\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.score\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"anothertag:thevalue\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.satisfied\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"anothertag:thevalue\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.tolerating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"anothertag:thevalue\",\"unit:result\"]},{\"metric\":\"test.test_apdex.apdex.frustrating\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"anothertag:thevalue\",\"unit:result\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.percent\",\"points\":[[1483232461,50]],\"type\":\"count\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.percent\",\"points\":[[1483232461,50]],\"type\":\"count\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\"]},{\"metric\":\"test.test_counter.counter.value\",\"points\":[[1483232461,2]],\"type\":\"count\",\"interval\":10,\"tags\":[\"unit:none\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"key1:value1\",\"key2:value2\",\"item:item1:value1\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.percent\",\"points\":[[1483232461,50]],\"type\":\"count\",\"interval\":10,\"tags\":[\"key1:value1\",\"key2:value2\",\"item:item1:value1\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"key1:value1\",\"key2:value2\",\"item:item2:value2\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.percent\",\"points\":[[1483232461,50]],\"type\":\"count\",\"interval\":10,\"tags\":[\"key1:value1\",\"key2:value2\",\"item:item2:value2\",\"unit:none\"]},{\"metric\":\"test.test_counter.counter.value\",\"points\":[[1483232461,2]],\"type\":\"count\",\"interval\":10,\"tags\":[\"key1:value1\",\"key2:value2\",\"unit:none\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"key1:value1\",\"key2:value2\",\"item:item1:value1\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.percent\",\"points\":[[1483232461,50]],\"type\":\"count\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"key1:value1\",\"key2:value2\",\"item:item1:value1\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"key1:value1\",\"key2:value2\",\"item:item2:value2\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.percent\",\"points\":[[1483232461,50]],\"type\":\"count\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"key1:value1\",\"key2:value2\",\"item:item2:value2\",\"unit:none\"]},{\"metric\":\"test.test_counter.counter.value\",\"points\":[[1483232461,2]],\"type\":\"count\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"key1:value1\",\"key2:value2\",\"unit:none\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\"]},{\"metric\":\"test.test_counter__items.counter.total\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\"]},{\"metric\":\"test.test_counter.counter.value\",\"points\":[[1483232461,2]],\"type\":\"count\",\"interval\":10,\"tags\":[\"unit:none\"]}]}";
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
            var expected = "{\"series\":[{\"metric\":\"test.test_counter.counter.value\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"unit:none\"]}]}";
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
            var expected = "{\"series\":[{\"metric\":\"test.test_counter.counter.value\",\"points\":[[1483232461,1]],\"type\":\"count\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]}]}";
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
            var expected = "{\"series\":[{\"metric\":\"test.test_gauge.gauge.value\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]}]}";
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
            var expected = "{\"series\":[{\"metric\":\"test.gauge-group.gauge.value\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_histogram.histogram.samples\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.last\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.count.hist\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.sum\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.min\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.max\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.mean\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.median\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.stddev\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p999\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p99\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p98\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p95\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p75\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.user.last\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.user.min\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.user.max\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_histogram.histogram.samples\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.last\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.count.hist\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.sum\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.min\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.max\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.mean\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.median\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.stddev\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p999\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p99\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p98\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p95\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.p75\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.user.last\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.user.min\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]},{\"metric\":\"test.test_histogram.histogram.user.max\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\"]}]}";
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
            var expected = "{\"series\":[{\"metric\":\"test.test_meter.meter.count.meter\",\"points\":[[1483232461,1]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_meter.meter.count.meter\",\"points\":[[1483232461,1]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_meter__items.meter.count.meter\",\"points\":[[1483232461,1]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.percent\",\"points\":[[1483232461,50]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.count.meter\",\"points\":[[1483232461,1]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.percent\",\"points\":[[1483232461,50]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.count.meter\",\"points\":[[1483232461,2]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"unit:none\",\"unit_rate:ms\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_meter__items.meter.count.meter\",\"points\":[[1483232461,1]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.percent\",\"points\":[[1483232461,50]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item1:value1\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.count.meter\",\"points\":[[1483232461,1]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter__items.meter.percent\",\"points\":[[1483232461,50]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"item:item2:value2\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.count.meter\",\"points\":[[1483232461,2]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate1m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate5m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]},{\"metric\":\"test.test_meter.meter.rate15m\",\"points\":[[1483232461,0]],\"type\":\"rate\",\"interval\":10,\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_rate:ms\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_timer.timer.count.meter\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.rate1m\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.rate5m\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.rate15m\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.samples\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.last\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.count.hist\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.sum\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.min\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.max\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.mean\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.median\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.stddev\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p999\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p99\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p98\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p95\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p75\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.user.last\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.user.min\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.user.max\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]}]}";
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
                "{\"series\":[{\"metric\":\"test.test_timer.timer.count.meter\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.rate1m\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.rate5m\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.rate15m\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.samples\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.last\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.count.hist\",\"points\":[[1483232461,1]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.sum\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.min\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.max\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.mean\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.median\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.stddev\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p999\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p99\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p98\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p95\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.p75\",\"points\":[[1483232461,1000]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.user.last\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.user.min\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]},{\"metric\":\"test.test_timer.timer.user.max\",\"points\":[[1483232461,0]],\"type\":\"gauge\",\"tags\":[\"host:server1\",\"env:staging\",\"unit:none\",\"unit_dur:ms\",\"unit_rate:ms\"]}]}";
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
            var settings = new DatadogOptions();
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();

            await using var ms = new MemoryStream();
            await using (var packer = new MetricSnapshotDatadogJsonWriter(ms, flushInterval))
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