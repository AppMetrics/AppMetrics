// <copyright file="MetricSnapshotInfluxDBLineProtocolWriterTests.cs" company="App Metrics Contributors">
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
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Serialization;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.InfluxDB.Facts
{
    public class MetricSnapshotInfluxDbLineProtocolWriterTests
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
                "test__test_apdex,mtype=apdex,unit=result samples=0i,score=0,satisfied=0i,tolerating=0i,frustrating=0i 1483232461000000000\n";
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
                "test__test_apdex,host=server1,env=staging,mtype=apdex,unit=result samples=0i,score=0,satisfied=0i,tolerating=0i,frustrating=0i 1483232461000000000\n";
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
                "test__test_apdex,key1=value1,key2=value2,mtype=apdex,unit=result samples=0i,score=0,satisfied=0i,tolerating=0i,frustrating=0i 1483232461000000000\n";
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
                "test__test_apdex,host=server1,env=staging,anothertag=thevalue,mtype=apdex,unit=result samples=0i,score=0,satisfied=0i,tolerating=0i,frustrating=0i 1483232461000000000\n";
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
                "test__test_counter__items,item=item1:value1,mtype=counter,unit=none total=1i,percent=50 1483232461000000000\ntest__test_counter__items,item=item2:value2,mtype=counter,unit=none total=1i,percent=50 1483232461000000000\ntest__test_counter,mtype=counter,unit=none value=2i 1483232461000000000\n";
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
                "test__test_counter__items,key1=value1,key2=value2,item=item1:value1,mtype=counter,unit=none total=1i,percent=50 1483232461000000000\ntest__test_counter__items,key1=value1,key2=value2,item=item2:value2,mtype=counter,unit=none total=1i,percent=50 1483232461000000000\ntest__test_counter,key1=value1,key2=value2,mtype=counter,unit=none value=2i 1483232461000000000\n";
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
                "test__test_counter__items,host=server1,env=staging,key1=value1,key2=value2,item=item1:value1,mtype=counter,unit=none total=1i,percent=50 1483232461000000000\ntest__test_counter__items,host=server1,env=staging,key1=value1,key2=value2,item=item2:value2,mtype=counter,unit=none total=1i,percent=50 1483232461000000000\ntest__test_counter,host=server1,env=staging,key1=value1,key2=value2,mtype=counter,unit=none value=2i 1483232461000000000\n";
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
                "test__test_counter__items,item=item1:value1,mtype=counter,unit=none total=1i 1483232461000000000\ntest__test_counter__items,item=item2:value2,mtype=counter,unit=none total=1i 1483232461000000000\ntest__test_counter,mtype=counter,unit=none value=2i 1483232461000000000\n";
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
            var expected = "test__test_counter,mtype=counter,unit=none value=1i 1483232461000000000\n";
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
            var expected = "test__test_counter,host=server1,env=staging,mtype=counter,unit=none value=1i 1483232461000000000\n";
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
            var expected = "test__test_gauge,mtype=gauge,unit=none value=1 1483232461000000000\n";
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
            var expected = "test__gauge-group,host=server1,env=staging,mtype=gauge,unit=none value=1 1483232461000000000\n";
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
                "test__test_histogram,mtype=histogram,unit=none samples=1i,last=1000,count.hist=1i,sum=1000,min=1000,max=1000,mean=1000,median=1000,stddev=0,p999=1000,p99=1000,p98=1000,p95=1000,p75=1000,user.last=\"client1\",user.min=\"client1\",user.max=\"client1\" 1483232461000000000\n";
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
                "test__test_histogram,host=server1,env=staging,mtype=histogram,unit=none samples=1i,last=1000,count.hist=1i,sum=1000,min=1000,max=1000,mean=1000,median=1000,stddev=0,p999=1000,p99=1000,p98=1000,p95=1000,p75=1000,user.last=\"client1\",user.min=\"client1\",user.max=\"client1\" 1483232461000000000\n";
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
            var expected = "test__test_meter,mtype=meter,unit=none,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0 1483232461000000000\n";
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
                "test__test_meter,host=server1,env=staging,mtype=meter,unit=none,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0 1483232461000000000\n";
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
                "test__test_meter__items,item=item1:value1,mtype=meter,unit=none,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0,percent=50 1483232461000000000\ntest__test_meter__items,item=item2:value2,mtype=meter,unit=none,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0,percent=50 1483232461000000000\ntest__test_meter,mtype=meter,unit=none,unit_rate=ms count.meter=2i,rate1m=0,rate5m=0,rate15m=0 1483232461000000000\n";
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
                "test__test_meter__items,host=server1,env=staging,item=item1:value1,mtype=meter,unit=none,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0,percent=50 1483232461000000000\ntest__test_meter__items,host=server1,env=staging,item=item2:value2,mtype=meter,unit=none,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0,percent=50 1483232461000000000\ntest__test_meter,host=server1,env=staging,mtype=meter,unit=none,unit_rate=ms count.meter=2i,rate1m=0,rate5m=0,rate15m=0 1483232461000000000\n";
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
                "test__test_timer,mtype=timer,unit=none,unit_dur=ms,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0,samples=1i,last=1000,count.hist=1i,sum=1000,min=1000,max=1000,mean=1000,median=1000,stddev=0,p999=1000,p99=1000,p98=1000,p95=1000,p75=1000,user.last=\"client1\",user.min=\"client1\",user.max=\"client1\" 1483232461000000000\n";
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
                "test__test_timer,host=server1,env=staging,mtype=timer,unit=none,unit_dur=ms,unit_rate=ms count.meter=1i,rate1m=0,rate5m=0,rate15m=0,samples=1i,last=1000,count.hist=1i,sum=1000,min=1000,max=1000,mean=1000,median=1000,stddev=0,p999=1000,p99=1000,p98=1000,p95=1000,p75=1000,user.last=\"client1\",user.min=\"client1\",user.max=\"client1\" 1483232461000000000\n";
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
            var settings = new MetricsInfluxDbLineProtocolOptions();
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();

            await using var sw = new StringWriter();
            await using (var packer = new MetricSnapshotInfluxDbLineProtocolWriter(sw, settings.MetricNameFormatter))
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