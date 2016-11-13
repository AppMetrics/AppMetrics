using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Reporting;
using App.Metrics.Scheduling;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Reporting
{
    public class ReporterTests : IClassFixture<MetricsReportingFixture>
    {
        private readonly MetricsReportingFixture _fixture;

        public ReporterTests(MetricsReportingFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task when_null_providers_doest_throw()
        {
            var factory = new ReportFactory();
            var scheduler = new Mock<IScheduler>();
            var reporter = new Reporter(factory, scheduler.Object);

            await reporter.RunReportsAsync(_fixture.Metrics, CancellationToken.None);
        }

        [Fact]
        public async Task when_provider_added_the_associated_metric_reporter_is_created()
        {
            var factory = new ReportFactory();
            var provider = new Mock<IReporterProvider>();
            var scheduler = new Mock<IScheduler>();
            var metricReporter = new Mock<IMetricReporter>();
            provider.Setup(p => p.CreateMetricReporter(It.IsAny<string>())).Returns(metricReporter.Object);
            factory.AddProvider(provider.Object);

            var reporter = new Reporter(factory, scheduler.Object);

            provider.Verify(p => p.CreateMetricReporter(It.IsAny<string>()), Times.Once);

            await reporter.RunReportsAsync(_fixture.Metrics, CancellationToken.None);
        }

        [Fact]
        public async Task schedules_reports_to_run_at_the_specified_interval()
        {
            var interval = TimeSpan.FromSeconds(60);
            var factory = new ReportFactory();
            var provider = new Mock<IReporterProvider>();
            var scheduler = new Mock<IScheduler>();
            var metricReporter = new Mock<IMetricReporter>();
            var settings = new Mock<IReporterSettings>();
            settings.Setup(s => s.ReportInterval).Returns(interval);
            metricReporter.Setup(r => r.ReportInterval).Returns(interval);
            provider.Setup(p => p.CreateMetricReporter(It.IsAny<string>())).Returns(metricReporter.Object);
            provider.Setup(p => p.Settings).Returns(settings.Object);
            factory.AddProvider(provider.Object);

            var reporter = new Reporter(factory, scheduler.Object);

            await reporter.RunReportsAsync(_fixture.Metrics, CancellationToken.None);

            scheduler.Verify(p => p.Interval(interval, It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}