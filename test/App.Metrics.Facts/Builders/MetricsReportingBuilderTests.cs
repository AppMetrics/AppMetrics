// <copyright file="MetricsReportingBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.Filtering;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class MetricsReportingBuilderTests
    {
        [Fact]
        public void Cannot_set_null_reporter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new MetricsBuilder().Report.Using(reporter: null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Can_use_reporter_instance()
        {
            // Arrange
            var reporter = new TestReporter();
            var builder = new MetricsBuilder().Report.Using(reporter);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TestReporter);
        }

        [Fact]
        public void Can_use_reporter_of_type()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.Using<TestReporter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TestReporter);
        }

        [Fact]
        public void Formatter_set_to_reporter_default_when_not_specified()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.Using<TestReporter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TestReporter);
            metrics.Reporters.First(r => r.GetType() == typeof(TestReporter)).Formatter.Should().BeOfType<MetricsTextOutputFormatter>();
        }

        [Fact]
        public void Formatter_set_to_user_specified_when_specified()
        {
            // Arrange
            var builder = new MetricsBuilder().OutputMetrics.AsJson().Report.Using<TestReporter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TestReporter);
            metrics.Reporters.First(r => r.GetType() == typeof(TestReporter)).Formatter.Should().BeOfType<MetricsJsonOutputFormatter>();
        }

        [Fact]
        public void Can_use_reporter_of_type_and_override_flushinterval()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.Using<TestReporter>(flushInterval: TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();
            var reporter = (metrics.Reporters as MetricsReporterCollection)?.GetType<TestReporter>();

            // Assert
            reporter?.FlushInterval.Should().Be(TimeSpan.FromDays(1));
        }

        [Fact]
        public void Can_use_reporter_of_type_and_use_specific_output_formatter()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.Using<TestReporter>(formatter: new MetricsJsonOutputFormatter());

            // Act
            var metrics = builder.Build();
            var reporter = (metrics.Reporters as MetricsReporterCollection)?.GetType<TestReporter>();

            // Assert
            reporter?.Formatter.Should().BeOfType(typeof(MetricsJsonOutputFormatter));
        }

        [Fact]
        public void Can_use_reporter_of_type_and_use_specific_metrics_filter()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var builder = new MetricsBuilder().Report.Using<TestReporter>(filter: filter);

            // Act
            var metrics = builder.Build();
            var reporter = (metrics.Reporters as MetricsReporterCollection)?.GetType<TestReporter>();

            // Assert
            reporter?.Filter.Should().BeSameAs(filter);
        }

        [Fact]
        public void Can_use_reporter_of_type_and_use_specific_metrics_filter_and_flushinterval()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var builder = new MetricsBuilder().Report.Using<TestReporter>(filter: filter, flushInterval: TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();
            var reporter = (metrics.Reporters as MetricsReporterCollection)?.GetType<TestReporter>();

            // Assert
            reporter?.Filter.Should().BeSameAs(filter);
            reporter?.FlushInterval.Should().Be(TimeSpan.FromDays(1));
        }

        [Fact]
        public void Can_use_reporter_of_type_and_use_specific_metrics_formatter_and_flushinterval()
        {
            // Arrange
            var formatter = new MetricsJsonOutputFormatter();
            var builder = new MetricsBuilder().Report.Using<TestReporter>(formatter: formatter, flushInterval: TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();
            var reporter = (metrics.Reporters as MetricsReporterCollection)?.GetType<TestReporter>();

            // Assert
            reporter?.Formatter.Should().BeOfType(typeof(MetricsJsonOutputFormatter));
            reporter?.FlushInterval.Should().Be(TimeSpan.FromDays(1));
        }

        [Fact]
        public void Can_use_reporter_instance_and_use_specific_metrics_filter_and_flushinterval_and_formatter()
        {
            // Arrange
            var formatter = new MetricsJsonOutputFormatter();
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var builder = new MetricsBuilder().Report.Using<TestReporter>(formatter: formatter, filter: filter, flushInterval: TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();
            var reporter = (metrics.Reporters as MetricsReporterCollection)?.GetType<TestReporter>();

            // Assert
            reporter?.Filter.Should().BeSameAs(filter);
            reporter?.FlushInterval.Should().Be(TimeSpan.FromDays(1));
            reporter?.Formatter.Should().BeOfType(typeof(MetricsJsonOutputFormatter));
        }
    }
}