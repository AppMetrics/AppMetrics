// <copyright file="HealthReportingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Facts.TestHelpers;
using App.Metrics.Health.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts.Builders
{
    public class HealthReportingBuilderTests
    {
        [Fact]
        public void Cannot_set_null_reporter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new HealthBuilder().Report.Using(reporter: null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Can_use_reporter_instance()
        {
            // Arrange
            var reporter = new TestReporter();
            var builder = new HealthBuilder().Report.Using(reporter);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TestReporter);
        }

        [Fact]
        public void Can_use_reporter_of_type()
        {
            // Arrange
            var builder = new HealthBuilder().Report.Using<TestReporter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TestReporter);
        }

        [Fact]
        public void Can_use_reporter_of_type_and_override_flushinterval()
        {
            // Arrange
            var builder = new HealthBuilder().Report.Using<TestReporter>(reportInterval: TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();
            var reporter = (metrics.Reporters as HealthReporterCollection)?.GetType<TestReporter>();

            // Assert
            reporter?.ReportInterval.Should().Be(TimeSpan.FromDays(1));
        }
    }
}