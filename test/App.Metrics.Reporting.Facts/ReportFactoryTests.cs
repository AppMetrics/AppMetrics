// <copyright file="ReportFactoryTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Configuration;
using App.Metrics.Scheduling;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Reporting.Facts
{
    public class ReportFactoryTests
    {
        [Fact]
        public void Can_create_reporter_with_custom_scheduler()
        {
            var metrics = new Mock<IMetrics>();
            var scheduler = new Mock<IScheduler>();
            var loggerFactory = new LoggerFactory();
            var reportFactory = new ReportFactory(metrics.Object, loggerFactory);

            Action action = () =>
            {
                var unused = reportFactory.CreateReporter(scheduler.Object);
            };

            action.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void Can_create_reporter_with_default_scheduler()
        {
            var metrics = new Mock<IMetrics>();
            var loggerFactory = new LoggerFactory();
            var reportFactory = new ReportFactory(metrics.Object, loggerFactory);

            Action action = () =>
            {
                var unused = reportFactory.CreateReporter();
            };

            action.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void Imetrics_is_required()
        {
            Action action = () =>
            {
                var unused = new ReportFactory(null, new LoggerFactory());
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Logger_factory_is_required()
        {
            var metrics = new Mock<IMetrics>();
            var options = new AppMetricsOptions();

            Action action = () =>
            {
                var unused = new ReportFactory(metrics.Object, null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }
    }
}