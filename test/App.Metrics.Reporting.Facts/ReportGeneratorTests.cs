using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Reporting.Facts.Fixtures;
using Moq;
using Xunit;

namespace App.Metrics.Reporting.Facts
{
    public class ReportGeneratorTests : IClassFixture<MetricsFixture>
    {
        private readonly IMetrics Metrics;
        private readonly DefaultReportGenerator _reportGenerator;

        public ReportGeneratorTests(MetricsFixture fixture)
        {
            Metrics = fixture.Metrics;
            _reportGenerator = new DefaultReportGenerator();
        }

        [Fact]
        public async Task reports_the_start_and_end_of_the_report()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartReport(It.IsAny<IMetrics>()));
            metricReporter.Setup(x => x.EndReport(It.IsAny<IMetrics>()));
            var tags = new MetricTags().With("tag1", "value1");
            var token = CancellationToken.None;

            await _reportGenerator.Generate(metricReporter.Object, Metrics, tags, token);

            metricReporter.Verify(p => p.StartReport(It.IsAny<IMetrics>()), Times.Once);
            metricReporter.Verify(p => p.EndReport(It.IsAny<IMetrics>()), Times.Once);
        }


        [Fact]
        public async Task reports_the_start_and_end_of_each_metric_type()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartMetricTypeReport(It.IsAny<Type>()));
            var tags = new MetricTags().With("tag1", "value1");
            var token = CancellationToken.None;

            await _reportGenerator.Generate(metricReporter.Object, Metrics, tags, token);

            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(CounterValueSource)), Times.Once);
            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(MeterValueSource)), Times.Once);
            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(TimerValueSource)), Times.Once);
            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(GaugeValueSource)), Times.Once);
            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(HistogramValueSource)), Times.Once);
        }

        [Fact]
        public async Task reports_environment_info()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartMetricTypeReport(typeof(EnvironmentInfo)));
            metricReporter.Setup(x => x.EndMetricTypeReport(typeof(EnvironmentInfo)));
            metricReporter.Setup(x => x.ReportEnvironment(It.IsAny<EnvironmentInfo>()));
            var tags = new MetricTags().With("tag1", "value1");
            var token = CancellationToken.None;

            await _reportGenerator.Generate(metricReporter.Object, Metrics, tags, token);

            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(EnvironmentInfo)), Times.Once);
            metricReporter.Verify(p => p.EndMetricTypeReport(typeof(EnvironmentInfo)), Times.Once);
            metricReporter.Verify(p => p.ReportEnvironment(It.IsAny<EnvironmentInfo>()), Times.Once);
        }

        [Fact]
        public async Task can_disable_reporting_environment_info()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartMetricTypeReport(typeof(EnvironmentInfo)));
            metricReporter.Setup(x => x.EndMetricTypeReport(typeof(EnvironmentInfo)));
            metricReporter.Setup(x => x.ReportEnvironment(It.IsAny<EnvironmentInfo>()));
            var tags = new MetricTags().With("tag1", "value1");
            var token = CancellationToken.None;
            var filter = new DefaultMetricsFilter().WithEnvironmentInfo(false);

            await _reportGenerator.Generate(metricReporter.Object, Metrics, filter, tags, token);

            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(EnvironmentInfo)), Times.Never);
            metricReporter.Verify(p => p.EndMetricTypeReport(typeof(EnvironmentInfo)), Times.Never);
            metricReporter.Verify(p => p.ReportEnvironment(It.IsAny<EnvironmentInfo>()), Times.Never);
        }

        [Fact]
        public async Task reports_health()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartMetricTypeReport(typeof(HealthStatus)));
            metricReporter.Setup(x => x.EndMetricTypeReport(typeof(HealthStatus)));
            metricReporter.Setup(x => x.ReportHealth(It.IsAny<IEnumerable<HealthCheck.Result>>(), It.IsAny<IEnumerable<HealthCheck.Result>>()));
            var tags = new MetricTags().With("tag1", "value1");
            var token = CancellationToken.None;

            await _reportGenerator.Generate(metricReporter.Object, Metrics, tags, token);

            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(HealthStatus)), Times.Once);
            metricReporter.Verify(p => p.EndMetricTypeReport(typeof(HealthStatus)), Times.Once);
            metricReporter.Verify(p => p.ReportHealth(It.IsAny<IEnumerable<HealthCheck.Result>>(), It.IsAny<IEnumerable<HealthCheck.Result>>()), Times.Once);
        }

        [Fact]
        public async Task can_disable_reporting_health()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartMetricTypeReport(typeof(HealthStatus)));
            metricReporter.Setup(x => x.EndMetricTypeReport(typeof(HealthStatus)));
            metricReporter.Setup(x => x.ReportHealth(It.IsAny<IEnumerable<HealthCheck.Result>>(), It.IsAny<IEnumerable<HealthCheck.Result>>()));
            var tags = new MetricTags().With("tag1", "value1");
            var token = CancellationToken.None;
            var filter = new DefaultMetricsFilter().WithHealthChecks(false);

            await _reportGenerator.Generate(metricReporter.Object, Metrics, filter, tags, token);

            metricReporter.Verify(p => p.StartMetricTypeReport(typeof(HealthStatus)), Times.Never);
            metricReporter.Verify(p => p.EndMetricTypeReport(typeof(HealthStatus)), Times.Never);
            metricReporter.Verify(p => p.ReportHealth(It.IsAny<IEnumerable<HealthCheck.Result>>(), It.IsAny<IEnumerable<HealthCheck.Result>>()), Times.Never);
        }
    }
}