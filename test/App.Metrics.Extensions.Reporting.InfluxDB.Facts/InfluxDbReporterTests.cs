using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace App.Metrics.Extensions.Middleware.Integration.Facts
{
    public class InfluxDbReporterTests
    {
        [Fact]
        public void can_report_apdex()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var gauge = new ApdexMetric(SamplingType.ExponentiallyDecaying, 1028, 0.015, clock, false);
            var apdexValueSource = new ApdexValueSource("test apdex",
                ConstantValue.Provider(gauge.Value), MetricTags.None, false);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", apdexValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_apdex samples=0i,score=0,satisfied=0i,tolerating=0i,frustrating=0i\n");
        }

        [Fact]
        public void can_report_counters()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new CounterMetric();
            counter.Increment(1);
            var counterValueSource = new CounterValueSource("test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None, MetricTags.None);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_counter value=1i\n");
        }

        [Fact]
        public void can_report_counter_with_items()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new CounterMetric();
            counter.Increment(new MetricItem().With("item1", "value1"), 1);
            counter.Increment(new MetricItem().With("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource("test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None, MetricTags.None);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_counter__items,item=item1:value1 total=1i,percent=50\ntest__test_counter__items,item=item2:value2 total=1i,percent=50\ntest__test_counter value=2i\n");
        }

        [Fact]
        public void can_report_counter_with_items_with_option_not_to_report_percentage()
        {
            var metricsMock = new Mock<IMetrics>();
            var counter = new CounterMetric();
            counter.Increment(new MetricItem().With("item1", "value1"), 1);
            counter.Increment(new MetricItem().With("item2", "value2"), 1);
            var counterValueSource = new CounterValueSource("test counter",
                ConstantValue.Provider(counter.Value),
                Unit.None, MetricTags.None,reportItemPercentages:false);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", counterValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_counter__items,item=item1:value1 total=1i\ntest__test_counter__items,item=item2:value2 total=1i\ntest__test_counter value=2i\n");
        }

        [Fact]
        public void can_report_gauges()
        {
            var metricsMock = new Mock<IMetrics>();
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource("test gauge",
                ConstantValue.Provider(gauge.Value),
                Unit.None, MetricTags.None);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", gaugeValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_gauge value=1\n");
        }

        [Fact]
        public void can_report_health()
        {
            var metricsMock = new Mock<IMetrics>();
            var healthyChecks = new[]
            {
                new HealthCheck.Result("healthy check", HealthCheckResult.Healthy("healthy message"))
            }.AsEnumerable();

            var degradedChecks = new[]
            {
                new HealthCheck.Result("degraded check", HealthCheckResult.Degraded("degraded message"))
            }.AsEnumerable();

            var unhealthyChecks = new[]
            {
                new HealthCheck.Result("unhealthy check", HealthCheckResult.Unhealthy("unhealthy message"))
            }.AsEnumerable();
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportHealth(new GlobalMetricTags(), healthyChecks, degradedChecks, unhealthyChecks);

            payloadBuilder.PayloadFormatted().Should().Be("health value=3i\nhealth_checks__unhealhty,health_check=unhealthy\\ check value=\"unhealthy message\"\nhealth_checks__degraded,health_check=degraded\\ check value=\"degraded message\"\nhealth_checks__healthy,health_check=healthy\\ check value=\"healthy message\"\n");
        }

        [Fact]
        public void when_disposed_clears_playload()
        {
            var payloadBuilderMock = new Mock<ILineProtocolPayloadBuilder>();
            payloadBuilderMock.Setup(p => p.Clear());
            var reporter = CreateReporter(payloadBuilderMock.Object);

            reporter.Dispose();

            payloadBuilderMock.Verify(p => p.Clear(), Times.Once);
        }

        [Fact]
        public async Task on_end_report_clears_playload()
        {
            var metricsMock = new Mock<IMetrics>();
            var payloadBuilderMock = new Mock<ILineProtocolPayloadBuilder>();
            payloadBuilderMock.Setup(p => p.Clear());
            var reporter = CreateReporter(payloadBuilderMock.Object);

            await reporter.EndAndFlushReportRunAsync(metricsMock.Object).ConfigureAwait(false);

            payloadBuilderMock.Verify(p => p.Clear(), Times.Once);
        }

        [Fact]
        public void can_report_histograms()
        {
            var metricsMock = new Mock<IMetrics>();
            var histogram = new HistogramMetric(SamplingType.ExponentiallyDecaying, 1028, 0.015);
            histogram.Update(1000, "client1");
            var histogramValueSource = new HistogramValueSource("test histogram",
                ConstantValue.Provider(histogram.Value), Unit.None, MetricTags.None);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", histogramValueSource);

            payloadBuilder.PayloadFormatted()
                .Should()
                .Be(
                    "test__test_histogram samples=1i,last=1000,count.hist=1i,min=1000,max=1000,mean=1000,median=1000,stddev=0,p999=1000,p99=1000,p98=1000,p95=1000,p75=1000,user.last=\"client1\",user.min=\"client1\",user.max=\"client1\"\n");
        }

        [Fact]
        public void can_report_meters()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var meter = new MeterMetric(clock);
            meter.Mark(1);
            var meterValueSource = new MeterValueSource("test meter",
                ConstantValue.Provider(meter.Value), Unit.None, TimeUnit.Milliseconds, MetricTags.None);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", meterValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_meter count.meter=1i,rate1m=0,rate5m=0,rate15m=0,rate.mean=Infinity\n");
        }

        [Fact]
        public void can_report_meters_with_items()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var meter = new MeterMetric(clock);
            meter.Mark(new MetricItem().With("item1", "value1"), 1);
            meter.Mark(new MetricItem().With("item2", "value2"), 1);
            var meterValueSource = new MeterValueSource("test meter",
                ConstantValue.Provider(meter.Value), Unit.None, TimeUnit.Milliseconds, MetricTags.None);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", meterValueSource);

            payloadBuilder.PayloadFormatted().Should().Be("test__test_meter__items,item=item1:value1 count.meter=1i,rate1m=0,rate5m=0,rate15m=0,rate.mean=Infinity,percent=50\ntest__test_meter__items,item=item2:value2 count.meter=1i,rate1m=0,rate5m=0,rate15m=0,rate.mean=Infinity,percent=50\ntest__test_meter count.meter=2i,rate1m=0,rate5m=0,rate15m=0,rate.mean=Infinity\n");
        }

        [Fact]
        public void can_report_timers()
        {
            var metricsMock = new Mock<IMetrics>();
            var clock = new TestClock();
            var timer = new TimerMetric(SamplingType.ExponentiallyDecaying, 1028, 0.015, clock);
            timer.Record(1000, TimeUnit.Milliseconds, "client1");
            var timerValueSource = new TimerValueSource("test timer",
                ConstantValue.Provider(timer.Value), Unit.None, TimeUnit.Milliseconds, TimeUnit.Milliseconds, MetricTags.None);
            var payloadBuilder = new LineProtocolPayloadBuilder();
            var reporter = CreateReporter(payloadBuilder);

            reporter.StartReportRun(metricsMock.Object);
            reporter.ReportMetric("test", timerValueSource);

            payloadBuilder.PayloadFormatted()
                .Should()
                .Be(
                    "test__test_timer count.meter=1i,rate1m=0,rate5m=0,rate15m=0,rate.mean=Infinity,samples=1i,last=1000,count.hist=1i,min=1000,max=1000,mean=1000,median=1000,stddev=0,p999=1000,p99=1000,p98=1000,p95=1000,p75=1000,user.last=\"client1\",user.min=\"client1\",user.max=\"client1\"\n");
        }

        private static InfluxDbReporter CreateReporter(ILineProtocolPayloadBuilder payloadBuilder)
        {
            var lineProtocolClientMock = new Mock<ILineProtocolClient>();
            var reportInterval = TimeSpan.FromSeconds(1);
            var loggerFactory = new LoggerFactory();

            return new InfluxDbReporter(lineProtocolClientMock.Object,
                payloadBuilder, reportInterval, loggerFactory);
        }
    }
}