using System;
using App.Metrics.Configuration;
using App.Metrics.Reporting;
using App.Metrics.Scheduling.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Reporting
{
    public class ReportFactoryTests
    {
        [Fact]
        public void can_create_reporter_with_custom_scheduler()
        {
            var metrics = new Mock<IMetrics>();
            var options = new AppMetricsOptions();
            var scheduler = new Mock<IScheduler>();
            var loggerFactory = new LoggerFactory();
            var reportFactory = new ReportFactory(options, metrics.Object, loggerFactory);

            Action action = () =>
            {
                var unused = reportFactory.CreateReporter(scheduler.Object);
            };

            action.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void can_create_reporter_with_default_scheduler()
        {
            var metrics = new Mock<IMetrics>();
            var options = new AppMetricsOptions();
            var loggerFactory = new LoggerFactory();
            var reportFactory = new ReportFactory(options, metrics.Object, loggerFactory);

            Action action = () =>
            {
                var unused = reportFactory.CreateReporter();
            };

            action.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void imetrics_is_required()
        {
            Action action = () =>
            {
                var unused = new ReportFactory(new AppMetricsOptions(), null, new LoggerFactory());
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void logger_factory_is_required()
        {
            var metrics = new Mock<IMetrics>();
            var options = new AppMetricsOptions();

            Action action = () =>
            {
                var unused = new ReportFactory(options, metrics.Object, null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void options_are_required()
        {
            var metrics = new Mock<IMetrics>();

            Action action = () =>
            {
                var unused = new ReportFactory(null, metrics.Object, new LoggerFactory());
            };

            action.ShouldThrow<ArgumentNullException>();
        }
    }
}