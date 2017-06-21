// <copyright file="ReportGeneratorTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Reporting.Facts.Fixtures;
using Moq;
using Xunit;

namespace App.Metrics.Reporting.Facts
{
    public class ReportGeneratorTests : IClassFixture<MetricsReportingFixture>
    {
        private readonly MetricsReportingFixture _fixture;

        public ReportGeneratorTests(MetricsReportingFixture fixture) { _fixture = fixture; }

        [Fact]
        public async Task Reports_the_start_and_end_of_the_report()
        {
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(x => x.StartReportRun(It.IsAny<IMetrics>()));
            metricReporter.Setup(x => x.EndAndFlushReportRunAsync(It.IsAny<IMetrics>())).Returns(Task.FromResult(true));
            var token = CancellationToken.None;

            await _fixture.ReportGenerator.GenerateAsync(metricReporter.Object, _fixture.Metrics(), token);

            metricReporter.Verify(p => p.StartReportRun(It.IsAny<IMetrics>()), Times.Once);
            metricReporter.Verify(p => p.EndAndFlushReportRunAsync(It.IsAny<IMetrics>()), Times.Once);
        }
    }
}