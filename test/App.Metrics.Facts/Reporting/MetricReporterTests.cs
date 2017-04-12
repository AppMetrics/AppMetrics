using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.Reporting.Helpers;
using App.Metrics.Filtering;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Facts.Reporting
{
    public class MetricReporterTests
    {
        private readonly IMetrics _metrics;
        private readonly DefaultReportGenerator _reportGenerator;

        public MetricReporterTests()
        {
            // DEVNOTE: Don't want to share metrics between tests
            var fixture = new MetricsReportingFixture();
            _metrics = fixture.Metrics;
            _reportGenerator = new DefaultReportGenerator(new AppMetricsOptions(), new LoggerFactory());
        }

        [Fact]
        public async Task can_pack_metrics()
        {
            // Arrange
            var token = CancellationToken.None;
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder);
            var filter = new DefaultMetricsFilter().WithEnvironmentInfo(false);

            // Act
            await _reportGenerator.GenerateAsync(reporter, _metrics, filter, token);
            var payload = payloadBuilder.PayloadFormatted();

            // Assert
            payload.Should().
                    Be(
                        "application__test_counter tag1=value type=counter value=1i" + Environment.NewLine +
                        "application__test_gauge type=gauge value=8" + Environment.NewLine +
                        "application__test_histogram type=histogram samples=1i last=5 count.hist=1i sum=5 min=5 max=5 mean=5 median=5 stddev=0 p999=5 p99=5 p98=5 p95=5 p75=5" +
                        Environment.NewLine +
                        "application__test_meter tag2=value type=meter count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000" +
                        Environment.NewLine +
                        "application__test_timer type=timer count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000 samples=1i last=10 count.hist=1i sum=10 min=10 max=10 mean=10 median=10 stddev=0 p999=10 p99=10 p98=10 p95=10 p75=10" +
                        Environment.NewLine);
        }

        [Fact]
        public async Task can_pack_metrics_with_custom_histogram_keys()
        {
            // Arrange
            var token = CancellationToken.None;
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder, new CustomPackMetricDataKeys { Histogram = { {HistogramDataKeys.P75, "75th_percentile"}}});
            var filter = new DefaultMetricsFilter().WithEnvironmentInfo(false);

            // Act
            await _reportGenerator.GenerateAsync(reporter, _metrics, filter, token);
            var payload = payloadBuilder.PayloadFormatted();

            // Assert
            payload.Should().
                    Be(
                        "application__test_counter tag1=value type=counter value=1i" + Environment.NewLine +
                        "application__test_gauge type=gauge value=8" + Environment.NewLine +
                        "application__test_histogram type=histogram samples=1i last=5 count.hist=1i sum=5 min=5 max=5 mean=5 median=5 stddev=0 p999=5 p99=5 p98=5 p95=5 75th_percentile=5" +
                        Environment.NewLine +
                        "application__test_meter tag2=value type=meter count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000" +
                        Environment.NewLine +
                        "application__test_timer type=timer count.meter=1i rate1m=0 rate5m=0 rate15m=0 rate.mean=6000 samples=1i last=10 count.hist=1i sum=10 min=10 max=10 mean=10 median=10 stddev=0 p999=10 p99=10 p98=10 p95=10 75th_percentile=10" +
                        Environment.NewLine);
        }

        [Fact]
        public async Task can_pack_metrics_with_custom_meter_keys()
        {
            // Arrange
            var token = CancellationToken.None;
            var payloadBuilder = new TestPayloadBuilder();
            var reporter = new TestReporter(payloadBuilder, new CustomPackMetricDataKeys { Meter = { {MeterValueDataKeys.Rate1M, "1_min_rate" } } });
            var filter = new DefaultMetricsFilter().WithEnvironmentInfo(false);

            // Act
            await _reportGenerator.GenerateAsync(reporter, _metrics, filter, token);
            var payload = payloadBuilder.PayloadFormatted();

            // Assert
            payload.Should().
                    Be(
                        "application__test_counter tag1=value type=counter value=1i" + Environment.NewLine +
                        "application__test_gauge type=gauge value=8" + Environment.NewLine +
                        "application__test_histogram type=histogram samples=1i last=5 count.hist=1i sum=5 min=5 max=5 mean=5 median=5 stddev=0 p999=5 p99=5 p98=5 p95=5 p75=5" +
                        Environment.NewLine +
                        "application__test_meter tag2=value type=meter count.meter=1i 1_min_rate=0 rate5m=0 rate15m=0 rate.mean=6000" +
                        Environment.NewLine +
                        "application__test_timer type=timer count.meter=1i 1_min_rate=0 rate5m=0 rate15m=0 rate.mean=6000 samples=1i last=10 count.hist=1i sum=10 min=10 max=10 mean=10 median=10 stddev=0 p999=10 p99=10 p98=10 p95=10 p75=10" +
                        Environment.NewLine);
        }        
    }
}