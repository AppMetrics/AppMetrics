using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Formatting.StatsD;
using App.Metrics.Formatting.StatsD.Internal;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Serialization;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.StatsD.Facts
{
    public class MetricSnapshotStatsDStringSerializerTest
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
                "test.test_apdex.apdex.samples:0|g|#unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.score:0|g|#unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.satisfied:0|g|#unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.tolerating:0|g|#unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.frustrating:0|g|#unit:result,timestamp:1483232461";
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
                "test.test_apdex.apdex.samples:0|g|#host:server1,env:staging,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.score:0|g|#host:server1,env:staging,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.satisfied:0|g|#host:server1,env:staging,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.tolerating:0|g|#host:server1,env:staging,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.frustrating:0|g|#host:server1,env:staging,unit:result,timestamp:1483232461";
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
                "test.test_apdex.apdex.samples:0|g|#key1:value1,key2:value2,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.score:0|g|#key1:value1,key2:value2,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.satisfied:0|g|#key1:value1,key2:value2,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.tolerating:0|g|#key1:value1,key2:value2,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.frustrating:0|g|#key1:value1,key2:value2,unit:result,timestamp:1483232461";
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
                "test.test_apdex.apdex.samples:0|g|#host:server1,env:staging,anothertag:thevalue,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.score:0|g|#host:server1,env:staging,anothertag:thevalue,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.satisfied:0|g|#host:server1,env:staging,anothertag:thevalue,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.tolerating:0|g|#host:server1,env:staging,anothertag:thevalue,unit:result,timestamp:1483232461\n" +
                "test.test_apdex.apdex.frustrating:0|g|#host:server1,env:staging,anothertag:thevalue,unit:result,timestamp:1483232461";
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
                "test.test_counter__items.counter.item1:value1.total:1|c|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c|#unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:2|c|#unit:none,timestamp:1483232461";
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
        public async Task Can_report_counter_with_custom_sample_rate()
        {
            // Arrange
            var expected =
                "test.test_counter__items.counter.item1:value1.total:1|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:2|c|@0.5|#unit:none,timestamp:1483232461";
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
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected, 0.5);
        }

        [Fact]
        public async Task Counter_with_custom_sample_rate_with_multiple_data_point_should_sample_correctly()
        {
            // Arrange
            var expected =
                "test.test_counter__items.counter.item1:value1.total:4|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.percent:100|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:4|c|@0.5|#unit:none,timestamp:1483232461";
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty);

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected, 0.5);

        }

        [Fact]
        public async Task Counter_with_custom_sample_rate_with_multiple_MetricsDataValueSource_should_sample_correctly()
        {
            // Arrange
            var expected =
                "test.test_counter__items.counter.item1:value1.total:1|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.percent:100|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:1|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.total:1|c|@0.5|#unit:none,timestamp:1483232463\n" +
                "test.test_counter__items.counter.item1:value1.percent:100|c|@0.5|#unit:none,timestamp:1483232463\n" +
                "test.test_counter.counter.value:1|c|@0.5|#unit:none,timestamp:1483232463\n" +
                "test.test_counter__items.counter.item1:value1.total:1|c|@0.5|#unit:none,timestamp:1483232465\n" +
                "test.test_counter__items.counter.item1:value1.percent:100|c|@0.5|#unit:none,timestamp:1483232465\n" +
                "test.test_counter.counter.value:1|c|@0.5|#unit:none,timestamp:1483232465";

            var sources = new List<MetricsDataValueSource>();
            var timeStamp = _timestamp;

            // Act
            sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            sources.Add(CreateMetricsDataValueSource(ref timeStamp));

            // Assert
            await AssertExpectedLineProtocolString(sources, expected, 0.5);

            MetricsDataValueSource CreateMetricsDataValueSource(ref DateTime timestamp)
            {
                var counter = new DefaultCounterMetric();
                counter.Increment(new MetricSetItem("item1", "value1"), 1);
                var counterValueSource = new CounterValueSource(
                    "test counter",
                    ConstantValue.Provider(counter.Value),
                    Unit.None,
                    MetricTags.Empty);
                var valueSource = CreateValueSource("test", counters: counterValueSource);
                var result = new MetricsDataValueSource(timestamp, new[] { valueSource });
                timestamp += TimeSpan.FromSeconds(1);
                return result;
            }
        }

        [Fact]
        public async Task Can_report_counter_with_custom_sample_rate_through_tag()
        {
            // Arrange
            var expected =
                "test.test_counter__items.counter.item1:value1.total:1|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c|@0.5|#unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:2|c|@0.5|#unit:none,timestamp:1483232461";
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                new MetricTags(new[] { "sampleRate" }, new[] { "0.5" }));

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
                "test.test_counter__items.counter.item1:value1.total:1|c|#key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c|#key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c|#key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c|#key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:2|c|#key1:value1,key2:value2,unit:none,timestamp:1483232461";
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
                "test.test_counter__items.counter.item1:value1.total:1|c|#host:server1,env:staging,key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c|#host:server1,env:staging,key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c|#host:server1,env:staging,key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c|#host:server1,env:staging,key1:value1,key2:value2,unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:2|c|#host:server1,env:staging,key1:value1,key2:value2,unit:none,timestamp:1483232461";
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
                "test.test_counter__items.counter.item1:value1.total:1|c|#unit:none,timestamp:1483232461\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c|#unit:none,timestamp:1483232461\n" +
                "test.test_counter.counter.value:2|c|#unit:none,timestamp:1483232461";
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
            var expected = "test.test_counter.counter.value:1|c|#unit:none,timestamp:1483232461";
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
            var expected = "test.test_counter.counter.value:1|c|#host:server1,env:staging,unit:none,timestamp:1483232461";
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
            var expected = "test.test_gauge.gauge.value:1|g|#unit:none,timestamp:1483232461";
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
            var expected = "test.gauge-group.gauge.value:1|g|#host:server1,env:staging,unit:none,timestamp:1483232461";
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
                "test.test_histogram.histogram.samples:1|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.last:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.count.hist:1|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.sum:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.min:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.max:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.mean:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.median:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.stddev:0|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p999:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p99:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p98:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p95:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p75:1000|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.user.last:0|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.user.min:0|h|#unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.user.max:0|h|#unit:none,timestamp:1483232461";
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
                "test.test_histogram.histogram.samples:1|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.last:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.count.hist:1|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.sum:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.min:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.max:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.mean:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.median:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.stddev:0|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p999:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p99:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p98:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p95:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.p75:1000|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.user.last:0|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.user.min:0|h|#host:server1,env:staging,unit:none,timestamp:1483232461\n" +
                "test.test_histogram.histogram.user.max:0|h|#host:server1,env:staging,unit:none,timestamp:1483232461";
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
            var expected = 
                "test.test_meter.meter.count.meter:1|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate1m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate5m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate15m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461";
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
                "test.test_meter.meter.count.meter:1|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate1m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate5m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate15m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461";
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
                "test.test_meter__items.meter.item1:value1.count.meter:1|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.rate1m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.rate5m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.rate15m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.percent:50|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.count.meter:1|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.rate1m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.rate5m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.rate15m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.percent:50|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.count.meter:2|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate1m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate5m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate15m:0|m|#unit:none,unit_rate:ms,timestamp:1483232461";
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
                "test.test_meter__items.meter.item1:value1.count.meter:1|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.rate1m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.rate5m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.rate15m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item1:value1.percent:50|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.count.meter:1|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.rate1m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.rate5m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.rate15m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter__items.meter.item2:value2.percent:50|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.count.meter:2|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate1m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate5m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_meter.meter.rate15m:0|m|#host:server1,env:staging,unit:none,unit_rate:ms,timestamp:1483232461";
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
                "test.test_timer.timer.count.meter:1|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.rate1m:0|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.rate5m:0|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.rate15m:0|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.samples:1|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.last:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.count.hist:1|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.sum:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.min:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.max:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.mean:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.median:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.stddev:0|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p999:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p99:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p98:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p95:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p75:1000|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.user.last:0|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.user.min:0|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.user.max:0|ms|#unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461";
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
                "test.test_timer.timer.count.meter:1|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.rate1m:0|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.rate5m:0|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.rate15m:0|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.samples:1|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.last:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.count.hist:1|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.sum:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.min:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.max:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.mean:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.median:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.stddev:0|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p999:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p99:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p98:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p95:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.p75:1000|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.user.last:0|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.user.min:0|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461\n" +
                "test.test_timer.timer.user.max:0|ms|#host:server1,env:staging,unit:none,unit_dur:ms,unit_rate:ms,timestamp:1483232461";
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

        private async Task AssertExpectedLineProtocolString(MetricsDataValueSource dataValueSource, string expected, double sampleRate = 1.0)
            => await AssertExpectedLineProtocolString(new[] {dataValueSource}, expected, sampleRate);

        private async Task AssertExpectedLineProtocolString(IEnumerable<MetricsDataValueSource> dataValueSource, string expected, double sampleRate = 1.0)
        {
            var settings = new MetricsStatsDOptions
            {
                WriteTags = true,
                WriteTimestamp = true,
                DefaultSampleRate = sampleRate
            };
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();

            await using var ms = new MemoryStream();
            await using (var packer = 
                new MetricSnapshotStatsDStringWriter(
                    ms, 
                    new StatsDPointSampler(settings), 
                    settings))
            {
                foreach (var source in dataValueSource)
                {
                    serializer.Serialize(packer, source, fields);
                }
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
