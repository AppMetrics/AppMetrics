// <copyright file="ReporterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core.Scheduling;
using App.Metrics.Reporting.Facts.Fixtures;
using App.Metrics.Reporting.Facts.TestHelpers;
using App.Metrics.Reporting.Internal;
using App.Metrics.Scheduling;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Reporting.Facts
{
    public class ReporterTests : IClassFixture<MetricsReportingFixture>
    {
        private readonly MetricsReportingFixture _fixture;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public ReporterTests(MetricsReportingFixture fixture) { _fixture = fixture; }

        [Fact]
        public void Can_generate_report_successfully()
        {
            var factory = SetupReportFactory();
            factory.AddProvider(new TestReportProvider(true, TimeSpan.FromMilliseconds(10)));
            var scheduler = new DefaultTaskScheduler();
            var reporter = new DefaultReporter(factory, _fixture.Metrics(), scheduler, _loggerFactory);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunReports(_fixture.Metrics(), token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void Imetrics_is_required()
        {
            Action action = () =>
            {
                var scheduler = new DefaultTaskScheduler();
                var metrics = new Mock<IMetrics>();
                var factory = SetupReportFactory(metrics.Object);
                var unused = new DefaultReporter(factory, null, scheduler, _loggerFactory);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Logger_factory_is_required()
        {
            Action action = () =>
            {
                var scheduler = new DefaultTaskScheduler();
                var factory = SetupReportFactory();
                var unused = new DefaultReporter(factory, _fixture.Metrics(), scheduler, null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Logger_factory_is_required_when_instantiating_default_report_generator()
        {
            Action action = () =>
            {
                var unused = new DefaultReportGenerator(null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Report_factory_is_required()
        {
            Action action = () =>
            {
                var loggerFactory = new LoggerFactory();
                var scheduler = new DefaultTaskScheduler();
                var unused = new DefaultReporter(null, _fixture.Metrics(), scheduler, loggerFactory);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Scheduler_is_required()
        {
            Action action = () =>
            {
                var factory = SetupReportFactory();
                var unused = new DefaultReporter(factory, _fixture.Metrics(), null, _loggerFactory);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Schedules_reports_to_run_at_the_specified_interval()
        {
            var interval = TimeSpan.FromSeconds(60);
            var loggerFactory = new LoggerFactory();
            var factory = SetupReportFactory();
            var provider = new Mock<IReporterProvider>();
            var scheduler = new Mock<IScheduler>();
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(r => r.GetType()).Returns(typeof(IMetricReporter));
            var settings = new Mock<IReporterSettings>();
            settings.Setup(s => s.ReportInterval).Returns(interval);
            metricReporter.Setup(r => r.ReportInterval).Returns(interval);
            provider.Setup(p => p.CreateMetricReporter(It.IsAny<string>(), It.IsAny<ILoggerFactory>())).Returns(metricReporter.Object);
            factory.AddProvider(provider.Object);

            var metrics = _fixture.Metrics();

            var reporter = new DefaultReporter(factory, metrics, scheduler.Object, loggerFactory);

            reporter.RunReports(metrics, CancellationToken.None);

            scheduler.Verify(
                p => p.Interval(interval, TaskCreationOptions.LongRunning, It.IsAny<Action>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void When_metric_report_fails_should_not_throw()
        {
            var factory = SetupReportFactory();
            factory.AddProvider(new TestReportProvider(false, TimeSpan.FromMilliseconds(10), new Exception()));
            var scheduler = new DefaultTaskScheduler();
            var metrics = _fixture.Metrics();

            var reporter = new DefaultReporter(factory, metrics, scheduler, _loggerFactory);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunReports(metrics, token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void When_metric_reporter_fails_continues_to_retry()
        {
            var factory = SetupReportFactory();
            factory.AddProvider(new TestReportProvider(false, TimeSpan.FromMilliseconds(10)));
            var scheduler = new DefaultTaskScheduler();
            var metrics = _fixture.Metrics();
            var reporter = new DefaultReporter(factory, metrics, scheduler, _loggerFactory);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunReports(metrics, token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void When_null_providers_doest_throw()
        {
            var factory = SetupReportFactory();
            var scheduler = new Mock<IScheduler>();
            var metrics = _fixture.Metrics();
            var reporter = new DefaultReporter(factory, metrics, scheduler.Object, _loggerFactory);

            reporter.RunReports(metrics, CancellationToken.None);
        }

        [Fact]
        public void When_provider_added_the_associated_metric_reporter_is_created()
        {
            var factory = SetupReportFactory();
            var provider = new Mock<IReporterProvider>();
            var scheduler = new Mock<IScheduler>();
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(r => r.GetType()).Returns(typeof(IMetricReporter));
            provider.Setup(p => p.CreateMetricReporter(It.IsAny<string>(), It.IsAny<ILoggerFactory>())).Returns(metricReporter.Object);
            factory.AddProvider(provider.Object);
            var metrics = _fixture.Metrics();

            var reporter = new DefaultReporter(factory, metrics, scheduler.Object, _loggerFactory);

            provider.Verify(p => p.CreateMetricReporter(It.IsAny<string>(), It.IsAny<ILoggerFactory>()), Times.Once);

            reporter.RunReports(metrics, CancellationToken.None);
        }

        private ReportFactory SetupReportFactory(IMetrics metrics = null)
        {
            if (metrics == null)
            {
                metrics = _fixture.Metrics();
            }

            return new ReportFactory(metrics, new LoggerFactory());
        }
    }
}