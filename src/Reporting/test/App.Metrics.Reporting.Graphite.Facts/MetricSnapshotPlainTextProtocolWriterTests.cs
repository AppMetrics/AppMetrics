// <copyright file="MetricSnapshotPlainTextProtocolWriterTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Formatters.Graphite;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Serialization;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Graphite.Facts
{
    public class MetricSnapshotPlainTextProtocolWriterTests
    {
        private const string MultidimensionalMetricNameSuffix = "|host:server1,env:staging";
        private readonly IReservoir _defaultReservoir = new DefaultForwardDecayingReservoir();
        private readonly MetricTags _tags = new MetricTags(new[] { "host", "env" }, new[] { "server1", "staging" });
        private readonly DateTime _timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);

        [Fact]
        public async Task Can_report_apdex()
        {
            // Arrange
            var expected =
                "apdex.test.test_apdex.Samples 0 1483232461\napdex.test.test_apdex.Score 0.00 1483232461\napdex.test.test_apdex.Satisfied 0 1483232461\napdex.test.test_apdex.Tolerating 0 1483232461\napdex.test.test_apdex.Frustrating 0 1483232461\n";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex",
                ConstantValue.Provider(apdex.Value),
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_apdex__when_multidimensional()
        {
            // Arrange
            var expected =
                "env.staging.apdex.test.test_apdex.host.server1.Samples 0 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.Score 0.00 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.Satisfied 0 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.Tolerating 0 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.Frustrating 0 1483232461\n";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(apdex.Value),
                _tags);

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_apdex_with_tags()
        {
            // Arrange
            var expected =
                "apdex.test.test_apdex.key1.value1.key2.value2.Samples 0 1483232461\napdex.test.test_apdex.key1.value1.key2.value2.Score 0.00 1483232461\napdex.test.test_apdex.key1.value1.key2.value2.Satisfied 0 1483232461\napdex.test.test_apdex.key1.value1.key2.value2.Tolerating 0 1483232461\napdex.test.test_apdex.key1.value1.key2.value2.Frustrating 0 1483232461\n";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex",
                ConstantValue.Provider(apdex.Value),
                new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" }));

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_apdex_with_tags_when_multidimensional()
        {
            // Arrange
            var expected =
                "env.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.Samples 0 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.Score 0.00 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.Satisfied 0 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.Tolerating 0 1483232461\nenv.staging.apdex.test.test_apdex.host.server1.anothertag.thevalue.Frustrating 0 1483232461\n";
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(apdex.Value),
                MetricTags.Concat(_tags, new MetricTags("anothertag", "thevalue")));

            // Act
            var valueSource = CreateValueSource("test", apdexScores: apdexValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items()
        {
            // Arrange
            var expected =
                "counter.test.test_counter-SetItem.item.item1_value1.Total 1 1483232461\ncounter.test.test_counter-SetItem.item.item1_value1.Percent 50.00 1483232461\ncounter.test.test_counter-SetItem.item.item2_value2.Total 1 1483232461\ncounter.test.test_counter-SetItem.item.item2_value2.Percent 50.00 1483232461\ncounter.test.test_counter.Value 2 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items_and_tags()
        {
            // Arrange
            var expected =
                "counter.test.test_counter-SetItem.key1.value1.key2.value2.item.item1_value1.Total 1 1483232461\ncounter.test.test_counter-SetItem.key1.value1.key2.value2.item.item1_value1.Percent 50.00 1483232461\ncounter.test.test_counter-SetItem.key1.value1.key2.value2.item.item2_value2.Total 1 1483232461\ncounter.test.test_counter-SetItem.key1.value1.key2.value2.item.item2_value2.Percent 50.00 1483232461\ncounter.test.test_counter.key1.value1.key2.value2.Value 2 1483232461\n";
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
           await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items_tags_when_multidimensional()
        {
            // Arrange
            var expected =
                "env.staging.counter.test.test_counter-SetItem.host.server1.key1.value1.key2.value2.item.item1_value1.Total 1 1483232461\nenv.staging.counter.test.test_counter-SetItem.host.server1.key1.value1.key2.value2.item.item1_value1.Percent 50.00 1483232461\nenv.staging.counter.test.test_counter-SetItem.host.server1.key1.value1.key2.value2.item.item2_value2.Total 1 1483232461\nenv.staging.counter.test.test_counter-SetItem.host.server1.key1.value1.key2.value2.item.item2_value2.Percent 50.00 1483232461\nenv.staging.counter.test.test_counter.host.server1.key1.value1.key2.value2.Value 2 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_counter_with_items_with_option_not_to_report_percentage()
        {
            // Arrange
            var expected =
                "counter.test.test_counter-SetItem.item.item1_value1.Total 1 1483232461\ncounter.test.test_counter-SetItem.item.item2_value2.Total 1 1483232461\ncounter.test.test_counter.Value 2 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_counters()
        {
            // Arrange
            var expected = "counter.test.test_counter.Value 1 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_counters__when_multidimensional()
        {
            // Arrange
            var expected = "env.staging.counter.test.test_counter.host.server1.Value 1 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_gauges()
        {
            // Arrange
            var expected = "gauge.test.test_gauge.Value 1.00 1483232461\n";
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource(
                "test gauge",
                ConstantValue.Provider(gauge.Value),
                Unit.None,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", gauges: gaugeValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_gauges__when_multidimensional()
        {
            // Arrange
            var expected = "env.staging.gauge.test.gauge-group.host.server1.Value 1.00 1483232461\n";
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource(
                "gauge-group" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(gauge.Value),
                Unit.None,
                _tags);

            // Act
            var valueSource = CreateValueSource("test", gauges: gaugeValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_histograms()
        {
            // Arrange
            var expected =
                "histogram.test.test_histogram.Samples 1 1483232461\nhistogram.test.test_histogram.Last 1000.00 1483232461\nhistogram.test.test_histogram.Count 1 1483232461\nhistogram.test.test_histogram.Sum 1000.00 1483232461\nhistogram.test.test_histogram.Min 1000.00 1483232461\nhistogram.test.test_histogram.Max 1000.00 1483232461\nhistogram.test.test_histogram.Mean 1000.00 1483232461\nhistogram.test.test_histogram.Median 1000.00 1483232461\nhistogram.test.test_histogram.StdDev 0.00 1483232461\nhistogram.test.test_histogram.Percentile-999 1000.00 1483232461\nhistogram.test.test_histogram.Percentile-99 1000.00 1483232461\nhistogram.test.test_histogram.Percentile-98 1000.00 1483232461\nhistogram.test.test_histogram.Percentile-95 1000.00 1483232461\nhistogram.test.test_histogram.Percentile-75 1000.00 1483232461\nhistogram.test.test_histogram.User-Last client1 1483232461\nhistogram.test.test_histogram.User-Min client1 1483232461\nhistogram.test.test_histogram.User-Max client1 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_histograms_when_multidimensional()
        {
            // Arrange
            var expected =
                "env.staging.histogram.test.test_histogram.host.server1.Samples 1 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Last 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Count 1 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Sum 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Min 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Max 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Mean 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Median 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.StdDev 0.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Percentile-999 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Percentile-99 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Percentile-98 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Percentile-95 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.Percentile-75 1000.00 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.User-Last client1 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.User-Min client1 1483232461\nenv.staging.histogram.test.test_histogram.host.server1.User-Max client1 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_meters()
        {
            // Arrange
            var expected = "meter.test.test_meter.Total 1 1483232461\nmeter.test.test_meter.Rate-1-Min 0.00 1483232461\nmeter.test.test_meter.Rate-5-Min 0.00 1483232461\nmeter.test.test_meter.Rate-15-Min 0.00 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_meters_when_multidimensional()
        {
            // Arrange
            var expected =
                "env.staging.meter.test.test_meter.host.server1.Total 1 1483232461\nenv.staging.meter.test.test_meter.host.server1.Rate-1-Min 0.00 1483232461\nenv.staging.meter.test.test_meter.host.server1.Rate-5-Min 0.00 1483232461\nenv.staging.meter.test.test_meter.host.server1.Rate-15-Min 0.00 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_meters_with_items()
        {
            // Arrange
            var expected =
                "meter.test.test_meter-SetItem.item.item1_value1.Total 1 1483232461\nmeter.test.test_meter-SetItem.item.item1_value1.Rate-1-Min 0.00 1483232461\nmeter.test.test_meter-SetItem.item.item1_value1.Rate-5-Min 0.00 1483232461\nmeter.test.test_meter-SetItem.item.item1_value1.Rate-15-Min 0.00 1483232461\nmeter.test.test_meter-SetItem.item.item1_value1.Percent 50.00 1483232461\nmeter.test.test_meter-SetItem.item.item2_value2.Total 1 1483232461\nmeter.test.test_meter-SetItem.item.item2_value2.Rate-1-Min 0.00 1483232461\nmeter.test.test_meter-SetItem.item.item2_value2.Rate-5-Min 0.00 1483232461\nmeter.test.test_meter-SetItem.item.item2_value2.Rate-15-Min 0.00 1483232461\nmeter.test.test_meter-SetItem.item.item2_value2.Percent 50.00 1483232461\nmeter.test.test_meter.Total 2 1483232461\nmeter.test.test_meter.Rate-1-Min 0.00 1483232461\nmeter.test.test_meter.Rate-5-Min 0.00 1483232461\nmeter.test.test_meter.Rate-15-Min 0.00 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_meters_with_items_tags_when_multidimensional()
        {
            // Arrange
            var expected =
                "env.staging.meter.test.test_meter-SetItem.host.server1.item.item1_value1.Total 1 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item1_value1.Rate-1-Min 0.00 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item1_value1.Rate-5-Min 0.00 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item1_value1.Rate-15-Min 0.00 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item1_value1.Percent 50.00 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item2_value2.Total 1 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item2_value2.Rate-1-Min 0.00 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item2_value2.Rate-5-Min 0.00 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item2_value2.Rate-15-Min 0.00 1483232461\nenv.staging.meter.test.test_meter-SetItem.host.server1.item.item2_value2.Percent 50.00 1483232461\nenv.staging.meter.test.test_meter.host.server1.Total 2 1483232461\nenv.staging.meter.test.test_meter.host.server1.Rate-1-Min 0.00 1483232461\nenv.staging.meter.test.test_meter.host.server1.Rate-5-Min 0.00 1483232461\nenv.staging.meter.test.test_meter.host.server1.Rate-15-Min 0.00 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_timers()
        {
            // Arrange
            var expected =
                "timer.test.test_timer.Total 1 1483232461\ntimer.test.test_timer.Rate-1-Min 0.00 1483232461\ntimer.test.test_timer.Rate-5-Min 0.00 1483232461\ntimer.test.test_timer.Rate-15-Min 0.00 1483232461\ntimer.test.test_timer.Samples 1 1483232461\ntimer.test.test_timer.Last 1000.00 1483232461\ntimer.test.test_timer.Count 1 1483232461\ntimer.test.test_timer.Sum 1000.00 1483232461\ntimer.test.test_timer.Min 1000.00 1483232461\ntimer.test.test_timer.Max 1000.00 1483232461\ntimer.test.test_timer.Mean 1000.00 1483232461\ntimer.test.test_timer.Median 1000.00 1483232461\ntimer.test.test_timer.StdDev 0.00 1483232461\ntimer.test.test_timer.Percentile-999 1000.00 1483232461\ntimer.test.test_timer.Percentile-99 1000.00 1483232461\ntimer.test.test_timer.Percentile-98 1000.00 1483232461\ntimer.test.test_timer.Percentile-95 1000.00 1483232461\ntimer.test.test_timer.Percentile-75 1000.00 1483232461\ntimer.test.test_timer.User-Last client1 1483232461\ntimer.test.test_timer.User-Min client1 1483232461\ntimer.test.test_timer.User-Max client1 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        [Fact]
        public async Task Can_report_timers__when_multidimensional()
        {
            // Arrange
            var expected =
                "env.staging.timer.test.test_timer.host.server1.Total 1 1483232461\nenv.staging.timer.test.test_timer.host.server1.Rate-1-Min 0.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Rate-5-Min 0.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Rate-15-Min 0.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Samples 1 1483232461\nenv.staging.timer.test.test_timer.host.server1.Last 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Count 1 1483232461\nenv.staging.timer.test.test_timer.host.server1.Sum 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Min 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Max 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Mean 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Median 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.StdDev 0.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Percentile-999 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Percentile-99 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Percentile-98 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Percentile-95 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.Percentile-75 1000.00 1483232461\nenv.staging.timer.test.test_timer.host.server1.User-Last client1 1483232461\nenv.staging.timer.test.test_timer.host.server1.User-Min client1 1483232461\nenv.staging.timer.test.test_timer.host.server1.User-Max client1 1483232461\n";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected);
        }

        private async Task AssertExpectedLineProtocolString(MetricsDataValueSource dataValueSource, string expected)
        {
            var settings = new MetricsGraphitePlainTextProtocolOptions();
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();
            fields.DefaultGraphiteMetricFieldNames();

            await using var sw = new StringWriter();
            await using (var packer = new MetricSnapshotGraphitePlainTextProtocolWriter(sw, settings.MetricPointTextWriter))
            {
                serializer.Serialize(packer, dataValueSource, fields);
            }

            sw.ToString().Should().Be(expected);
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