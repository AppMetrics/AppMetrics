// <copyright file="ReportRunnerTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Core.Apdex;
using App.Metrics.Core.Counter;
using App.Metrics.Core.Gauge;
using App.Metrics.Core.Histogram;
using App.Metrics.Core.Infrastructure;
using App.Metrics.Core.Meter;
using App.Metrics.Core.ReservoirSampling.ExponentialDecay;
using App.Metrics.Core.Timer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Reporting.Facts.TestHelpers;
using App.Metrics.ReservoirSampling;
using App.Metrics.Timer;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Reporting.Facts
{
    public class ReportRunnerTests
    {
        private const string MultidimensionalMetricNameSuffix = "|host:server1,env:staging";
        private readonly IReservoir _defaultReservoir = new DefaultForwardDecayingReservoir();
        private readonly MetricTags _tags = new MetricTags(new[] { "host", "env" }, new[] { "server1", "staging" });

        [Fact]
        public void Can_clear_payload()
        {
            var metricsMock = new Mock<IMetrics>();
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
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", meterValueSource);

            var sb = new StringBuilder();
            items.Item2.Payload().Format(sb);
            sb.ToString().Should().NotBeNullOrWhiteSpace();

            items.Item2.Clear();

            items.Item2.Payload().Should().BeNull();
        }

        [Fact]
        public void Can_report_apdex()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var gauge = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex",
                ConstantValue.Provider(gauge.Value),
                MetricTags.Empty,
                false);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", apdexValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be("test__test_apdex mtype=apdex unit=result samples=0i score=0 satisfied=0i tolerating=0i frustrating=0i" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_apdex__when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var gauge = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(gauge.Value),
                _tags,
                resetOnReporting: false);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", apdexValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_apdex host=server1 env=staging mtype=apdex unit=result samples=0i score=0 satisfied=0i tolerating=0i frustrating=0i" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_apdex_with_tags()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var gauge = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex",
                ConstantValue.Provider(gauge.Value),
                new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" }),
                false);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", apdexValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_apdex key1=value1 key2=value2 mtype=apdex unit=result samples=0i score=0 satisfied=0i tolerating=0i frustrating=0i" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_apdex_with_tags_when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var gauge = new DefaultApdexMetric(_defaultReservoir, clock, false);
            var apdexValueSource = new ApdexValueSource(
                "test apdex" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(gauge.Value),
                MetricTags.Concat(_tags, new MetricTags("anothertag", "thevalue")),
                resetOnReporting: false);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", apdexValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_apdex host=server1 env=staging anothertag=thevalue mtype=apdex unit=result samples=0i score=0 satisfied=0i tolerating=0i frustrating=0i" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_counter_with_items()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", counterValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_counter__items item=item1:value1 mtype=counter unit=none total=1i percent=50" + Environment.NewLine +
                      "test__test_counter__items item=item2:value2 mtype=counter unit=none total=1i percent=50" + Environment.NewLine +
                      "test__test_counter mtype=counter unit=none value=2i" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_counter_with_items_and_tags()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" }));
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", counterValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_counter__items key1=value1 key2=value2 item=item1:value1 mtype=counter unit=none total=1i percent=50" +
                      Environment.NewLine +
                      "test__test_counter__items key1=value1 key2=value2 item=item2:value2 mtype=counter unit=none total=1i percent=50" +
                      Environment.NewLine + "test__test_counter key1=value1 key2=value2 mtype=counter unit=none value=2i" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_counter_with_items_tags_when_multidimensional()
        {
            var counterTags = new MetricTags(new[] { "key1", "key2" }, new[] { "value1", "value2" });
            var metricsMock = new Mock<IMetrics>();
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Concat(_tags, counterTags));
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", counterValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_counter__items host=server1 env=staging key1=value1 key2=value2 item=item1:value1 mtype=counter unit=none total=1i percent=50" +
                      Environment.NewLine +
                      "test__test_counter__items host=server1 env=staging key1=value1 key2=value2 item=item2:value2 mtype=counter unit=none total=1i percent=50" +
                      Environment.NewLine + "test__test_counter host=server1 env=staging key1=value1 key2=value2 mtype=counter unit=none value=2i" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_counter_with_items_with_option_not_to_report_percentage()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty,
                reportItemPercentages: false);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", counterValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_counter__items item=item1:value1 mtype=counter unit=none total=1i" + Environment.NewLine +
                      "test__test_counter__items item=item2:value2 mtype=counter unit=none total=1i" + Environment.NewLine +
                      "test__test_counter mtype=counter unit=none value=2i" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_counters()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new DefaultCounterMetric();
            counter.Increment(1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", counterValueSource);

            items.Item2.PayloadFormatted().Should().Be("test__test_counter mtype=counter unit=none value=1i" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_counters__when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new DefaultCounterMetric();
            counter.Increment(1);
            var counterValueSource = new CounterValueSource(
                "test counter" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(counter.Value),
                Unit.None,
                _tags);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", counterValueSource);

            items.Item2.PayloadFormatted().Should().Be(
                "test__test_counter host=server1 env=staging mtype=counter unit=none value=1i" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_gauges()
        {
            var metricsMock = new Mock<IMetrics>();
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource(
                "test gauge",
                ConstantValue.Provider(gauge.Value),
                Unit.None,
                MetricTags.Empty);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", gaugeValueSource);

            items.Item2.PayloadFormatted().Should().Be("test__test_gauge mtype=gauge unit=none value=1" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_gauges__when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource(
                "gauge-group" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(gauge.Value),
                Unit.None,
                _tags);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", gaugeValueSource);

            items.Item2.PayloadFormatted().Should().Be(
                "test__gauge-group host=server1 env=staging mtype=gauge unit=none value=1" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_histograms()
        {
            var metricsMock = new Mock<IMetrics>();
            var histogram = new DefaultHistogramMetric(_defaultReservoir);
            histogram.Update(1000, "client1");
            var histogramValueSource = new HistogramValueSource(
                "test histogram",
                ConstantValue.Provider(histogram.Value),
                Unit.None,
                MetricTags.Empty);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", histogramValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_histogram mtype=histogram unit=none samples=1i last=1000 count.hist=1i sum=1000 min=1000 max=1000 mean=1000 median=1000 stddev=0 p999=1000 p99=1000 p98=1000 p95=1000 p75=1000 user.last=\"client1\" user.min=\"client1\" user.max=\"client1\"" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_histograms_when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
            var histogram = new DefaultHistogramMetric(_defaultReservoir);
            histogram.Update(1000, "client1");
            var histogramValueSource = new HistogramValueSource(
                "test histogram" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(histogram.Value),
                Unit.None,
                _tags);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", histogramValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_histogram host=server1 env=staging mtype=histogram unit=none samples=1i last=1000 count.hist=1i sum=1000 min=1000 max=1000 mean=1000 median=1000 stddev=0 p999=1000 p99=1000 p98=1000 p95=1000 p75=1000 user.last=\"client1\" user.min=\"client1\" user.max=\"client1\"" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_meters()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            meter.Mark(1);
            var meterValueSource = new MeterValueSource(
                "test meter",
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                MetricTags.Empty);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", meterValueSource);

            items.Item2.PayloadFormatted().Should().Be(
                "test__test_meter mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_meters_when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            meter.Mark(1);
            var meterValueSource = new MeterValueSource(
                "test meter" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                _tags);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", meterValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_meter host=server1 env=staging mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_meters_with_items()
        {
            var metricsMock = new Mock<IMetrics>();
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
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", meterValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_meter__items item=item1:value1 mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                      Environment.NewLine +
                      "test__test_meter__items item=item2:value2 mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                      Environment.NewLine + "test__test_meter mtype=meter unit=none unit_rate=ms count.meter=2i rate1m=0 rate5m=0 rate15m=0" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_meters_with_items_tags_when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
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
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", meterValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_meter__items host=server1 env=staging item=item1:value1 mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                      Environment.NewLine +
                      "test__test_meter__items host=server1 env=staging item=item2:value2 mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                      Environment.NewLine +
                      "test__test_meter host=server1 env=staging mtype=meter unit=none unit_rate=ms count.meter=2i rate1m=0 rate5m=0 rate15m=0" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_timers()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var timer = new DefaultTimerMetric(_defaultReservoir, clock);
            timer.Record(1000, TimeUnit.Milliseconds, "client1");
            var timerValueSource = new TimerValueSource(
                "test timer",
                ConstantValue.Provider(timer.Value),
                Unit.None,
                TimeUnit.Minutes,
                TimeUnit.Milliseconds,
                MetricTags.Empty);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", timerValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_timer mtype=timer unit=none unit_dur=ms unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 samples=1i last=1000 count.hist=1i sum=1000 min=1000 max=1000 mean=1000 median=1000 stddev=0 p999=1000 p99=1000 p98=1000 p95=1000 p75=1000 user.last=\"client1\" user.min=\"client1\" user.max=\"client1\"" +
                      Environment.NewLine);
        }

        [Fact]
        public void Can_report_timers__when_multidimensional()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var timer = new DefaultTimerMetric(_defaultReservoir, clock);
            timer.Record(1000, TimeUnit.Milliseconds, "client1");
            var timerValueSource = new TimerValueSource(
                "test timer" + MultidimensionalMetricNameSuffix,
                ConstantValue.Provider(timer.Value),
                Unit.None,
                TimeUnit.Minutes,
                TimeUnit.Milliseconds,
                _tags);
            var items = CreateReporterAndPayloadBuilder();

            items.Item1.StartReportRun(metricsMock.Object);
            items.Item1.ReportMetric("test", timerValueSource);

            items.Item2.PayloadFormatted().
                  Should().
                  Be(
                      "test__test_timer host=server1 env=staging mtype=timer unit=none unit_dur=ms unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 samples=1i last=1000 count.hist=1i sum=1000 min=1000 max=1000 mean=1000 median=1000 stddev=0 p999=1000 p99=1000 p98=1000 p95=1000 p75=1000 user.last=\"client1\" user.min=\"client1\" user.max=\"client1\"" +
                      Environment.NewLine);
        }

        // [Fact]
        // public async Task on_end_report_clears_playload()
        // {
        //     var metricsMock = new Mock<IMetrics>();
        //     var payloadBuilderMock = new Mock<IMetricPayloadBuilder<TestMetricPayload>>();
        //     payloadBuilderMock.Setup(p => p.Clear());
        //     var items = CreateReporterAndPayloadBuilder(payloadBuilderMock.Object);

        // await items.Item1.EndAndFlushReportRunAsync(metricsMock.Object).ConfigureAwait(false);

        // payloadBuilderMock.Verify(p => p.Clear(), Times.Once);
        // }

        // [Fact]
        // public void when_disposed_clears_playload()
        // {
        //     var payloadBuilderMock = new Mock<IMetricPayloadBuilder<TestMetricPayload>>();
        //     payloadBuilderMock.Setup(p => p.Clear());
        //     var items = CreateReporterAndPayloadBuilder();

        // items.Item1.Dispose();

        // payloadBuilderMock.Verify(p => p.Clear(), Times.Once);
        // }

#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
        private static (IMetricReporter, IMetricPayloadBuilder<TestMetricPayload>) CreateReporterAndPayloadBuilder()
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
        {
            var loggerFactory = new LoggerFactory();
            var settings = new TestReporterSettings();
            var payloadBuilder = new TestPayloadBuilder(settings.MetricNameFormatter, settings.DataKeys);

            var reporter = new ReportRunner<TestMetricPayload>(
                p =>
                {
                    p.Payload();
                    return Task.FromResult(true);
                },
                payloadBuilder,
                settings.ReportInterval,
                "Test Reporter",
                loggerFactory);

            return (reporter, payloadBuilder);
        }
    }
}