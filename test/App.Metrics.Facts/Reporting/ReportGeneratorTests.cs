using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Configuration;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filtering;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Reporting.Internal;
using App.Metrics.Tagging;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Reporting
{
    public class ReportGeneratorTests : IClassFixture<MetricsReportingFixture>
    {
        private readonly IMetrics _metrics;
        private readonly DefaultReportGenerator _reportGenerator;

        public ReportGeneratorTests(MetricsReportingFixture fixture)
        {
            _metrics = fixture.Metrics;
            _reportGenerator = new DefaultReportGenerator(new AppMetricsOptions(), new LoggerFactory());
        }

        [Fact]
        public async Task can_disable_reporting_environment_info()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.ReportEnvironment(It.IsAny<EnvironmentInfo>()));
            var token = CancellationToken.None;
            var filter = new DefaultMetricsFilter().WithEnvironmentInfo(false);

            await _reportGenerator.GenerateAsync(metricReporter.Object, _metrics, filter, token);

            metricReporter.Verify(p => p.ReportEnvironment(It.IsAny<EnvironmentInfo>()), Times.Never);
        }

        [Fact]
        public async Task can_disable_reporting_health()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(
                x => x.ReportHealth(
                    It.IsAny<GlobalMetricTags>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>()));
            var token = CancellationToken.None;
            var filter = new DefaultMetricsFilter().WithHealthChecks(false);

            await _reportGenerator.GenerateAsync(metricReporter.Object, _metrics, filter, token);

            metricReporter.Verify(
                p => p.ReportHealth(
                    It.IsAny<GlobalMetricTags>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>()),
                Times.Never);
        }

        [Fact]
        public async Task reports_environment_info()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.ReportEnvironment(It.IsAny<EnvironmentInfo>()));
            var token = CancellationToken.None;

            await _reportGenerator.GenerateAsync(metricReporter.Object, _metrics, token);

            metricReporter.Verify(p => p.ReportEnvironment(It.IsAny<EnvironmentInfo>()), Times.Once);
        }

        [Fact]
        public async Task reports_health()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(
                x => x.ReportHealth(
                    It.IsAny<GlobalMetricTags>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>()));
            var token = CancellationToken.None;

            await _reportGenerator.GenerateAsync(metricReporter.Object, _metrics, token);

            metricReporter.Verify(
                p => p.ReportHealth(
                    It.IsAny<GlobalMetricTags>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>(),
                    It.IsAny<IEnumerable<HealthCheck.Result>>()),
                Times.Once);
        }


        [Fact]
        public async Task reports_the_start_and_end_of_the_report()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartReportRun(It.IsAny<IMetrics>()));
            metricReporter.Setup(x => x.EndAndFlushReportRunAsync(It.IsAny<IMetrics>())).Returns(Task.FromResult(true));
            var token = CancellationToken.None;

            await _reportGenerator.GenerateAsync(metricReporter.Object, _metrics, token);

            metricReporter.Verify(p => p.StartReportRun(It.IsAny<IMetrics>()), Times.Once);
            metricReporter.Verify(p => p.EndAndFlushReportRunAsync(It.IsAny<IMetrics>()), Times.Once);
        }
    }
}