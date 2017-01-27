// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Configuration;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.Reporting.Helpers;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Reporting.Internal;
using App.Metrics.Scheduling;
using App.Metrics.Scheduling.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Reporting
{
    public class ReporterTests : IClassFixture<MetricsReportingFixture>
    {
        private readonly MetricsReportingFixture _fixture;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
        private readonly AppMetricsOptions _options = new AppMetricsOptions();


        public ReporterTests(MetricsReportingFixture fixture) { _fixture = fixture; }

        [Fact]
        public void can_generate_report_successfully()
        {
            var factory = SetupReportFactory();
            factory.AddProvider(new TestReportProvider(true, TimeSpan.FromMilliseconds(10)));
            var scheduler = new DefaultTaskScheduler();
            var reporter = new Reporter(new AppMetricsOptions(), factory, _fixture.Metrics, scheduler, _loggerFactory);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunReports(_fixture.Metrics, token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void imetrics_is_required()
        {
            Action action = () =>
            {
                var scheduler = new DefaultTaskScheduler();
                var metrics = new Mock<IMetrics>();
                var factory = SetupReportFactory(metrics.Object);
                var reporter = new Reporter(_options, factory, null, scheduler, _loggerFactory);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void logger_factory_is_required()
        {
            Action action = () =>
            {
                var scheduler = new DefaultTaskScheduler();
                var factory = SetupReportFactory();
                var reporter = new Reporter(_options, factory, _fixture.Metrics, scheduler, null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void logger_factory_is_required_when_instantiating_default_report_generator()
        {
            Action action = () =>
            {
                var generator = new DefaultReportGenerator(_options, null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void report_factory_is_required()
        {
            Action action = () =>
            {
                var loggerFactory = new LoggerFactory();
                var scheduler = new DefaultTaskScheduler();
                var reporter = new Reporter(_options, null, _fixture.Metrics, scheduler, loggerFactory);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void scheduler_is_required()
        {
            Action action = () =>
            {
                var factory = SetupReportFactory();
                var reporter = new Reporter(_options, factory, _fixture.Metrics, null, _loggerFactory);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void schedules_reports_to_run_at_the_specified_interval()
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

            var reporter = new Reporter(_options, factory, _fixture.Metrics, scheduler.Object, loggerFactory);

            reporter.RunReports(_fixture.Metrics, CancellationToken.None);

            scheduler.Verify(
                p => p.Interval(interval, TaskCreationOptions.LongRunning, It.IsAny<Action>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void when_metric_report_fails_should_not_throw()
        {
            var factory = SetupReportFactory();
            factory.AddProvider(new TestReportProvider(false, TimeSpan.FromMilliseconds(10), new Exception()));
            var scheduler = new DefaultTaskScheduler();
            var reporter = new Reporter(_options, factory, _fixture.Metrics, scheduler, _loggerFactory);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunReports(_fixture.Metrics, token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void when_metric_reporter_fails_continues_to_retry()
        {
            var factory = SetupReportFactory();
            factory.AddProvider(new TestReportProvider(false, TimeSpan.FromMilliseconds(10)));
            var scheduler = new DefaultTaskScheduler();
            var reporter = new Reporter(_options, factory, _fixture.Metrics, scheduler, _loggerFactory);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.RunReports(_fixture.Metrics, token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void when_null_providers_doest_throw()
        {
            var factory = SetupReportFactory();
            var scheduler = new Mock<IScheduler>();
            var reporter = new Reporter(_options, factory, _fixture.Metrics, scheduler.Object, _loggerFactory);

            reporter.RunReports(_fixture.Metrics, CancellationToken.None);
        }

        [Fact]
        public void when_provider_added_the_associated_metric_reporter_is_created()
        {
            var factory = SetupReportFactory();
            var provider = new Mock<IReporterProvider>();
            var scheduler = new Mock<IScheduler>();
            var metricReporter = new Mock<IMetricReporter>();
            metricReporter.Setup(r => r.GetType()).Returns(typeof(IMetricReporter));
            provider.Setup(p => p.CreateMetricReporter(It.IsAny<string>(), It.IsAny<ILoggerFactory>())).Returns(metricReporter.Object);
            factory.AddProvider(provider.Object);

            var reporter = new Reporter(_options, factory, _fixture.Metrics, scheduler.Object, _loggerFactory);

            provider.Verify(p => p.CreateMetricReporter(It.IsAny<string>(), It.IsAny<ILoggerFactory>()), Times.Once);

            reporter.RunReports(_fixture.Metrics, CancellationToken.None);
        }

        private ReportFactory SetupReportFactory(IMetrics metrics = null)
        {
            if (metrics == null)
            {
                metrics = _fixture.Metrics;
            }
            
            return new ReportFactory(_options, metrics, new LoggerFactory());
        }
    }
}