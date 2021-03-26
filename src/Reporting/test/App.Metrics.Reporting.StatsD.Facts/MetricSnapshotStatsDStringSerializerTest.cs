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
                "test.test_apdex.apdex.samples:0|c\n" +
                "test.test_apdex.apdex.score:0|g\n" +
                "test.test_apdex.apdex.satisfied:0|c\n" +
                "test.test_apdex.apdex.tolerating:0|c\n" +
                "test.test_apdex.apdex.frustrating:0|c";
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
                "test.test_apdex.apdex.samples:0|c\n" +
                "test.test_apdex.apdex.score:0|g\n" +
                "test.test_apdex.apdex.satisfied:0|c\n" +
                "test.test_apdex.apdex.tolerating:0|c\n" +
                "test.test_apdex.apdex.frustrating:0|c";
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
                "test.test_apdex.apdex.samples:0|c\n" +
                "test.test_apdex.apdex.score:0|g\n" +
                "test.test_apdex.apdex.satisfied:0|c\n" +
                "test.test_apdex.apdex.tolerating:0|c\n" +
                "test.test_apdex.apdex.frustrating:0|c";
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
                "test.test_apdex.apdex.samples:0|c\n" +
                "test.test_apdex.apdex.score:0|g\n" +
                "test.test_apdex.apdex.satisfied:0|c\n" +
                "test.test_apdex.apdex.tolerating:0|c\n" +
                "test.test_apdex.apdex.frustrating:0|c";
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
        public async Task Can_report_counter_with_custom_sample_rate_through_tag()
        {
            // Arrange
            var expected =
                "test.test_counter__items.counter.item1:value1.total:1|c\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c\n" +
                "test.test_counter.counter.value:2|c";
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                new MetricTags(new[] { "sampleRate" }, new[] { "1.0" }));

            // Act
            var valueSource = CreateValueSource("test", counters: counterValueSource);

            // Assert
            await AssertExpectedLineProtocolString(new MetricsDataValueSource(_timestamp, new[] { valueSource }), expected, 0.5);
        }

        [Fact]
        public async Task Can_report_counter_with_items()
        {
            // Arrange
            var expected =
                "test.test_counter__items.counter.item1:value1.total:1|c\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c\n" +
                "test.test_counter.counter.value:2|c";
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
                "test.test_counter__items.counter.item1:value1.total:1|c\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c\n" +
                "test.test_counter.counter.value:2|c";
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
                "test.test_counter__items.counter.item1:value1.total:1|c\n" +
                "test.test_counter__items.counter.item1:value1.percent:50|c\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c\n" +
                "test.test_counter__items.counter.item2:value2.percent:50|c\n" +
                "test.test_counter.counter.value:2|c";
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
                "test.test_counter__items.counter.item1:value1.total:1|c\n" +
                "test.test_counter__items.counter.item2:value2.total:1|c\n" +
                "test.test_counter.counter.value:2|c";
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
            var expected = "test.test_counter.counter.value:1|c";
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
            var expected = "test.test_counter.counter.value:1|c";
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
            var expected = "test.test_gauge.gauge.value:1|g";
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
            var expected = "test.gauge-group.gauge.value:1|g";
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
            var expected = "test.test_histogram.histogram.value:1000|h";
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
            var expected = "test.test_histogram.histogram.value:1000|h";
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
                "test.test_meter.meter.value:1|c";
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
            var expected = "test.test_meter.meter.value:1|c";
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
                "test.test_meter__items.meter.item1:value1.value:1|c\n" +
                "test.test_meter__items.meter.item2:value2.value:1|c\n" +
                "test.test_meter.meter.value:2|c";
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
                "test.test_meter__items.meter.item1:value1.value:1|c\n" +
                "test.test_meter__items.meter.item2:value2.value:1|c\n" +
                "test.test_meter.meter.value:2|c";
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
                "test.test_timer.timer.value:1000|ms";
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
            var expected = "test.test_timer.timer.value:1000|ms";
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

        [Fact]
        public async Task Counter_with_custom_sample_rate_with_multiple_data_point_should_sample_correctly()
        {
            // Arrange
            var sources = new List<MetricsDataValueSource>();
            var timeStamp = _timestamp;

            var expected =
                "test.test_counter.counter.value:4|c|@0.5";

            // Act
            for (var i = 0; i < 10000; ++i)
            {
                sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            }
            var emittedData = (await Serialize(sources, 0.5)).Split('\n');

            // Assert
            (emittedData.Length > 4500 && emittedData.Length < 5500).Should().BeTrue();
            emittedData[0].Should().Be(expected);

            MetricsDataValueSource CreateMetricsDataValueSource(ref DateTime timestamp)
            {
                var counter = new DefaultCounterMetric();
                counter.Increment(1);
                counter.Increment(1);
                counter.Increment(1);
                counter.Increment(1);
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
        public async Task Counter_with_custom_sample_rate_with_multiple_MetricsDataValueSource_should_sample_correctly()
        {
            // Arrange
            var sources = new List<MetricsDataValueSource>();
            var timeStamp = _timestamp;

            // Act
            for (var i = 0; i < 10000; ++i)
            {
                sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            }

            var emittedDataCount = (await Serialize(sources, 0.5)).Split('\n').Length;

            // Assert
            (emittedDataCount > 4500 && emittedDataCount < 5500).Should().BeTrue();

            MetricsDataValueSource CreateMetricsDataValueSource(ref DateTime timestamp)
            {
                var counter = new DefaultCounterMetric();
                counter.Increment(1);
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
        public async Task Meters_should_not_be_sampled()
        {
            // Arrange
            var sources = new List<MetricsDataValueSource>();
            var timeStamp = _timestamp;

            var expected =
                "test.test_meter.meter.value:4|c";

            // Act
            for (var i = 0; i < 100; ++i)
            {
                sources.Add(CreateMetricsDataValueSource(ref timeStamp));
            }
            var emittedData = (await Serialize(sources, 0.5)).Split('\n');

            // Assert
            emittedData.Length.Should().Be(100);
            emittedData[0].Should().Be(expected);

            MetricsDataValueSource CreateMetricsDataValueSource(ref DateTime timestamp)
            {
                var clock = new TestClock();
                var meter = new DefaultMeterMetric(clock);
                meter.Mark(1);
                meter.Mark(1);
                meter.Mark(1);
                meter.Mark(1);
                var meterValueSource = new MeterValueSource(
                    "test meter",
                    ConstantValue.Provider(meter.Value),
                    Unit.None,
                    TimeUnit.Milliseconds,
                    MetricTags.Empty);
                var valueSource = CreateValueSource("test", meters: meterValueSource);
                var result = new MetricsDataValueSource(timestamp, new[] { valueSource });
                timestamp += TimeSpan.FromSeconds(1);
                return result;
            }
        }

        private async Task<string> Serialize(
            IEnumerable<MetricsDataValueSource> dataValueSource,
            double sampleRate = 1.0)
        {
            var settings = new MetricsStatsDOptions
            {
                DefaultSampleRate = sampleRate
            };
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();

            await using (var ms = new MemoryStream())
            {
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
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private async Task AssertExpectedLineProtocolString(MetricsDataValueSource dataValueSource, string expected, double sampleRate = 1.0)
            => await AssertExpectedLineProtocolString(new[] { dataValueSource }, expected, sampleRate);

        private async Task AssertExpectedLineProtocolString(
            IEnumerable<MetricsDataValueSource> dataValueSource,
            string expected,
            double sampleRate = 1.0)
        {
            (await Serialize(dataValueSource, sampleRate)).Should().Be(expected);
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

            return new MetricsContextValueSource(
                context,
                gaugeValues,
                counterValues,
                meterValues,
                histogramValues,
                bucketHistogramValues,
                timerValues,
                bucketTimerValues,
                apdexScoreValues);
        }
    }
}