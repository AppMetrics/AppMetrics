// <copyright file="MetricsConsoleReporterBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Internal.NoOp;
using App.Metrics.Reporting.FactsCommon;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Console.Facts
{
    public class MetricsConsoleReporterBuilderTests
    {
        [Fact]
        public void Can_use_console_reporter_with_defaults()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.ToConsole();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is ConsoleMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<MetricsTextOutputFormatter>();
        }

        [Fact]
        public void Can_use_console_reporter_by_type_with_defaults()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.Using<ConsoleMetricsReporter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is ConsoleMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<MetricsTextOutputFormatter>();
        }

        [Fact]
        public void Can_use_console_reporter_with_setup_action_to_override_defaults()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var builder = new MetricsBuilder().Report.ToConsole(
                options =>
                {
                    options.MetricsOutputFormatter = new TestMetricsFormatter();
                    options.Filter = filter;
                    options.FlushInterval = TimeSpan.FromDays(1);
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is ConsoleMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(TimeSpan.FromDays(1));
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void When_using_console_reporter_with_setup_action_flush_interval_greater_than_zero_should_apply()
        {
            // Arrange
            var flushInterval = TimeSpan.FromSeconds(1);
            var builder = new MetricsBuilder().Report.ToConsole(
                options =>
                {
                    options.FlushInterval = flushInterval;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
        }

        [Fact]
        public void When_using_console_reporter_with_setup_action_flush_interval_equal_to_zero_should_apply_default_interval()
        {
            // Arrange
            var flushInterval = TimeSpan.Zero;
            var builder = new MetricsBuilder().Report.ToConsole(
                options =>
                {
                    options.FlushInterval = flushInterval;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
        }

        [Fact]
        public void When_using_console_reporter_with_setup_action_flush_interval_less_than_zero_should_throw()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var flushInterval = TimeSpan.FromSeconds(-1);
                var builder = new MetricsBuilder().Report.ToConsole(
                    options =>
                    {
                        options.FlushInterval = flushInterval;
                    });

                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Can_use_console_reporter_overriding_flush_interval()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.ToConsole(TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is ConsoleMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(TimeSpan.FromDays(1));
        }
    }
}