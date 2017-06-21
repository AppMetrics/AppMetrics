// <copyright file="MetricReporterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Core.Apdex;
using App.Metrics.Core.Counter;
using App.Metrics.Core.Filtering;
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
using App.Metrics.Reporting.Facts.Fixtures;
using App.Metrics.Reporting.Facts.TestHelpers;
using App.Metrics.ReservoirSampling;
using App.Metrics.Timer;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Reporting.Facts
{
    public class MetricReporterTests : IClassFixture<MetricsReportingFixture>
    {
        private const string MultidimensionalMetricNameSuffix = "|host:server1,env:staging";
        private readonly IReservoir _defaultReservoir = new DefaultForwardDecayingReservoir();
        private readonly MetricsReportingFixture _fixture;
        private readonly MetricTags _tags = new MetricTags(new[] { "host", "env" }, new[] { "server1", "staging" });

        public MetricReporterTests(MetricsReportingFixture fixture) { _fixture = fixture; }

        [Fact]
        public async Task Can_pack_metrics()
        {
            // Arrange
            var token = CancellationToken.None;
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);
            var filter = new DefaultMetricsFilter();
            var metrics = _fixture.Metrics();

            // Act
            await _fixture.ReportGenerator.GenerateAsync(reporter, metrics, filter, token);
            var payload = payloadBuilder.PayloadFormatted();

            // Assert
            payload.Should().
                    Be(
                        "application__test_counter tag1=value mtype=counter unit=req value=1i" + Environment.NewLine +
                        "application__test_gauge mtype=gauge unit=none value=8" + Environment.NewLine +
                        "application__test_histogram mtype=histogram unit=req samples=1i last=5 count.hist=1i sum=5 min=5 max=5 mean=5 median=5 stddev=0 p999=5 p99=5 p98=5 p95=5 p75=5" +
                        Environment.NewLine +
                        "application__test_meter tag2=value mtype=meter unit=none unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000" +
                        Environment.NewLine +
                        "application__test_timer mtype=timer unit=req unit_dur=ms unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000 samples=1i last=10 count.hist=1i sum=10 min=10 max=10 mean=10 median=10 stddev=0 p999=10 p99=10 p98=10 p95=10 p75=10" +
                        Environment.NewLine);
        }

        [Fact]
        public async Task Can_pack_metrics_with_custom_histogram_keys()
        {
            // Arrange
            var dataKeys = new MetricValueDataKeys(
                histogram: new Dictionary<HistogramValueDataKeys, string> { { HistogramValueDataKeys.P75, "75th_percentile" } });
            var token = CancellationToken.None;
            var payloadBuilder = new TestPayloadBuilder(dataKeys);
            var reporter = new TestReporter(
                payloadBuilder);
            var filter = new DefaultMetricsFilter();

            // Act
            await _fixture.ReportGenerator.GenerateAsync(reporter, _fixture.Metrics(), filter, token);
            var payload = payloadBuilder.PayloadFormatted();

            // Assert
            payload.Should().
                    Be(
                        "application__test_counter tag1=value mtype=counter unit=req value=1i" + Environment.NewLine +
                        "application__test_gauge mtype=gauge unit=none value=8" + Environment.NewLine +
                        "application__test_histogram mtype=histogram unit=req samples=1i last=5 count.hist=1i sum=5 min=5 max=5 mean=5 median=5 stddev=0 p999=5 p99=5 p98=5 p95=5 75th_percentile=5" +
                        Environment.NewLine +
                        "application__test_meter tag2=value mtype=meter unit=none unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000" +
                        Environment.NewLine +
                        "application__test_timer mtype=timer unit=req unit_dur=ms unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000 samples=1i last=10 count.hist=1i sum=10 min=10 max=10 mean=10 median=10 stddev=0 p999=10 p99=10 p98=10 p95=10 75th_percentile=10" +
                        Environment.NewLine);
        }

        [Fact]
        public async Task Can_pack_metrics_with_custom_meter_keys()
        {
            // Arrange
            var dataKeys = new MetricValueDataKeys(
                meter: new Dictionary<MeterValueDataKeys, string> { { MeterValueDataKeys.Rate1M, "1_min_rate" } });
            var token = CancellationToken.None;
            var payloadBuilder = new TestPayloadBuilder(dataKeys);
            var reporter = new TestReporter(payloadBuilder);
            var filter = new DefaultMetricsFilter();

            // Act
            await _fixture.ReportGenerator.GenerateAsync(reporter, _fixture.Metrics(), filter, token);
            var payload = payloadBuilder.PayloadFormatted();

            // Assert
            payload.Should().
                    Be(
                        "application__test_counter tag1=value mtype=counter unit=req value=1i" + Environment.NewLine +
                        "application__test_gauge mtype=gauge unit=none value=8" + Environment.NewLine +
                        "application__test_histogram mtype=histogram unit=req samples=1i last=5 count.hist=1i sum=5 min=5 max=5 mean=5 median=5 stddev=0 p999=5 p99=5 p98=5 p95=5 p75=5" +
                        Environment.NewLine +
                        "application__test_meter tag2=value mtype=meter unit=none unit_rate=min count.meter=1i 1_min_rate=0 rate5m=0 rate15m=0 rate.mean=6000" +
                        Environment.NewLine +
                        "application__test_timer mtype=timer unit=req unit_dur=ms unit_rate=min count.meter=1i 1_min_rate=0 rate5m=0 rate15m=0 rate.mean=6000 samples=1i last=10 count.hist=1i sum=10 min=10 max=10 mean=10 median=10 stddev=0 p999=10 p99=10 p98=10 p95=10 p75=10" +
                        Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", apdexValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_apdex mtype=apdex unit=result samples=0i score=0 satisfied=0i tolerating=0i frustrating=0i" +
                               Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", apdexValueSource);

            payloadBuilder.PayloadFormatted().
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", apdexValueSource);

            payloadBuilder.PayloadFormatted().
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", apdexValueSource);

            payloadBuilder.PayloadFormatted().
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_counter__items item=item1:value1 mtype=counter unit=none total=1i percent=50" + Environment.NewLine +
                               "test__test_counter__items item=item2:value2 mtype=counter unit=none total=1i percent=50" + Environment.NewLine +
                               "test__test_counter mtype=counter unit=none value=2i" + Environment.NewLine);
        }

        [Fact]
        public void Can_report_counter_with_items_and_custom_data_keys()
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
            var customDataKeys = new MetricValueDataKeys(
                counter: new Dictionary<CounterValueDataKeys, string>
                         {
                             { CounterValueDataKeys.SetItemPercent, "%" },
                             { CounterValueDataKeys.Total, "count" }
                         });
            var payloadBuilder = new TestPayloadBuilder(customDataKeys);

            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_counter__items key1=value1 key2=value2 item=item1:value1 mtype=counter unit=none count=1i %=50" +
                               Environment.NewLine +
                               "test__test_counter__items key1=value1 key2=value2 item=item2:value2 mtype=counter unit=none count=1i %=50" +
                               Environment.NewLine +
                               "test__test_counter key1=value1 key2=value2 mtype=counter unit=none value=2i" + Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_counter__items key1=value1 key2=value2 item=item1:value1 mtype=counter unit=none total=1i percent=50" +
                               Environment.NewLine +
                               "test__test_counter__items key1=value1 key2=value2 item=item2:value2 mtype=counter unit=none total=1i percent=50" +
                               Environment.NewLine +
                               "test__test_counter key1=value1 key2=value2 mtype=counter unit=none value=2i" + Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_counter__items host=server1 env=staging key1=value1 key2=value2 item=item1:value1 mtype=counter unit=none total=1i percent=50" +
                               Environment.NewLine +
                               "test__test_counter__items host=server1 env=staging key1=value1 key2=value2 item=item2:value2 mtype=counter unit=none total=1i percent=50" +
                               Environment.NewLine +
                               "test__test_counter host=server1 env=staging key1=value1 key2=value2 mtype=counter unit=none value=2i" +
                               Environment.NewLine);
        }

        [Fact]
        public void Can_report_counter_with_items_using_custom_key()
        {
            var dataKeys = new MetricValueDataKeys(
                counter: new Dictionary<CounterValueDataKeys, string> { { CounterValueDataKeys.MetricSetItemSuffix, " setitem" } });
            var metricsMock = new Mock<IMetrics>();
            var counter = new DefaultCounterMetric();
            counter.Increment(new MetricSetItem("item1", "value1"), 1);
            counter.Increment(new MetricSetItem("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource(
                "test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None,
                MetricTags.Empty);
            var payloadBuilder = new TestPayloadBuilder(dataKeys);
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_counter_setitem item=item1:value1 mtype=counter unit=none total=1i percent=50" + Environment.NewLine +
                               "test__test_counter_setitem item=item2:value2 mtype=counter unit=none total=1i percent=50" + Environment.NewLine +
                               "test__test_counter mtype=counter unit=none value=2i" + Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_counter mtype=counter unit=none value=1i" + Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().Should().Be(
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", gaugeValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_gauge mtype=gauge unit=none value=1" + Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", gaugeValueSource);

            payloadBuilder.PayloadFormatted().Should().Be(
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", histogramValueSource);

            payloadBuilder.PayloadFormatted().
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", histogramValueSource);

            payloadBuilder.PayloadFormatted().
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", meterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be("test__test_meter mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0" + Environment.NewLine);
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", meterValueSource);

            payloadBuilder.PayloadFormatted().
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", meterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_meter__items item=item1:value1 mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                               Environment.NewLine +
                               "test__test_meter__items item=item2:value2 mtype=meter unit=none unit_rate=ms count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                               Environment.NewLine +
                               "test__test_meter mtype=meter unit=none unit_rate=ms count.meter=2i rate1m=0 rate5m=0 rate15m=0" +
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
                TimeUnit.Minutes,
                _tags);
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", meterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_meter__items host=server1 env=staging item=item1:value1 mtype=meter unit=none unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                               Environment.NewLine +
                               "test__test_meter__items host=server1 env=staging item=item2:value2 mtype=meter unit=none unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                               Environment.NewLine +
                               "test__test_meter host=server1 env=staging mtype=meter unit=none unit_rate=min count.meter=2i rate1m=0 rate5m=0 rate15m=0" +
                               Environment.NewLine);
        }

        [Fact]
        public void Can_report_meters_with_items_using_custom_item_key()
        {
            var dataKeys = new MetricValueDataKeys(
                meter: new Dictionary<MeterValueDataKeys, string> { { MeterValueDataKeys.MetricSetItemSuffix, " setitem" } });
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            meter.Mark(new MetricSetItem("item1", "value1"), 1);
            meter.Mark(new MetricSetItem("item2", "value2"), 1);
            var meterValueSource = new MeterValueSource(
                "test meter",
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Minutes,
                MetricTags.Empty);
            var payloadBuilder = new TestPayloadBuilder(dataKeys);
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", meterValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_meter_setitem item=item1:value1 mtype=meter unit=none unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                               Environment.NewLine +
                               "test__test_meter_setitem item=item2:value2 mtype=meter unit=none unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 percent=50" +
                               Environment.NewLine +
                               "test__test_meter mtype=meter unit=none unit_rate=min count.meter=2i rate1m=0 rate5m=0 rate15m=0" +
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
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", timerValueSource);

            payloadBuilder.PayloadFormatted().
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
                Unit.Errors,
                TimeUnit.Minutes,
                TimeUnit.Milliseconds,
                _tags);
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", timerValueSource);

            payloadBuilder.PayloadFormatted().
                           Should().
                           Be(
                               "test__test_timer host=server1 env=staging mtype=timer unit=err unit_dur=ms unit_rate=min count.meter=1i rate1m=0 rate5m=0 rate15m=0 samples=1i last=1000 count.hist=1i sum=1000 min=1000 max=1000 mean=1000 median=1000 stddev=0 p999=1000 p99=1000 p98=1000 p95=1000 p75=1000 user.last=\"client1\" user.min=\"client1\" user.max=\"client1\"" +
                               Environment.NewLine);
        }
    }
}