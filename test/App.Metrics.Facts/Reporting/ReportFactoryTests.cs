// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Scheduling;
using App.Metrics.Reporting;
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
            var scheduler = new Mock<IScheduler>();
            var loggerFactory = new LoggerFactory();
            var reportFactory = new ReportFactory(metrics.Object, loggerFactory);

            Action action = () =>
            {
                var reporter = reportFactory.CreateReporter(scheduler.Object);
            };

            action.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void can_create_reporter_with_default_scheduler()
        {
            var metrics = new Mock<IMetrics>();
            var loggerFactory = new LoggerFactory();
            var reportFactory = new ReportFactory(metrics.Object, loggerFactory);

            Action action = () =>
            {
                var reporter = reportFactory.CreateReporter();
            };

            action.ShouldNotThrow<Exception>();
        }

        [Fact]
        public void imetrics_is_required()
        {
            Action action = () =>
            {
                var reportFactory = new ReportFactory(null, new LoggerFactory());
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void logger_factory_is_required()
        {
            var metrics = new Mock<IMetrics>();
            Action action = () =>
            {
                var reportFactory = new ReportFactory(metrics.Object, null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }
    }
}