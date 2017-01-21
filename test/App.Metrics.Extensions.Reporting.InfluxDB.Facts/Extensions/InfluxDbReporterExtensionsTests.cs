// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Extensions
{
    public class InfluxDbReporterExtensionsTests
    {
        [Fact]
        public void can_add_influxdb_provider_with_custom_settings()
        {
            var metricsMock = new Mock<IMetrics>();
            var factory = new ReportFactory(metricsMock.Object, new LoggerFactory());
            var settings = new InfluxDBReporterSettings
                           {
                               HttpPolicy = new HttpPolicy
                                            {
                                                BackoffPeriod = TimeSpan.FromMinutes(1)
                                            }
                           };
            Action action = () => { factory.AddInfluxDb(settings); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void can_add_influxdb_provider_with_custom_settings_and_filter()
        {
            var metricsMock = new Mock<IMetrics>();
            var factory = new ReportFactory(metricsMock.Object, new LoggerFactory());
            var settings = new InfluxDBReporterSettings
                           {
                               HttpPolicy = new HttpPolicy
                                            {
                                                BackoffPeriod = TimeSpan.FromMinutes(1)
                                            }
                           };
            Action action = () => { factory.AddInfluxDb(settings, new DefaultMetricsFilter()); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void can_add_influxdb_provider_with_filter()
        {
            var metricsMock = new Mock<IMetrics>();
            var factory = new ReportFactory(metricsMock.Object, new LoggerFactory());

            Action action = () => { factory.AddInfluxDb("test", new Uri("http://localhost"), new DefaultMetricsFilter()); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void can_add_influxdb_provider_without_filter()
        {
            var metricsMock = new Mock<IMetrics>();
            var factory = new ReportFactory(metricsMock.Object, new LoggerFactory());

            Action action = () => { factory.AddInfluxDb("test", new Uri("http://localhost")); };

            action.ShouldNotThrow();
        }
    }
}