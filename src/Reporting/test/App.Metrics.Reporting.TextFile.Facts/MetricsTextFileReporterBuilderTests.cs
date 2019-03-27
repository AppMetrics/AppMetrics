// <copyright file="MetricsTextFileReporterBuilderTests.cs" company="App Metrics Contributors">
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

namespace App.Metrics.Reporting.TextFile.Facts
{
    public class MetricsTextFileReporterBuilderTests
    {
        [Fact]
        public void Can_use_textfile_reporter_with_defaults()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.ToTextFile();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TextFileMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<MetricsTextOutputFormatter>();
        }

        [Fact]
        public void Can_use_textfile_reporter_by_type_with_defaults()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.Using<TextFileMetricsReporter>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TextFileMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<MetricsTextOutputFormatter>();
        }

        [Fact]
        public void Can_change_text_file_reporter_metrics_output_formatter()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.ToTextFile(
                options =>
                {
                    options.OutputPathAndFileName = @".\metrics.txt";
                    options.MetricsOutputFormatter = new TestMetricsFormatter();
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void Can_set_textfile_reporter_metrics_filter()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var builder = new MetricsBuilder().Report.ToTextFile(
                options =>
                {
                    options.OutputPathAndFileName = @".\metrics.txt";
                    options.Filter = filter;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
        }

        [Fact]
        public void Should_throw_when_using_setup_action_without_specifying_a_output_file()
        {
            // Arrange
            Action action = () =>
            {
                var builder = new MetricsBuilder().Report.ToTextFile(
                    options =>
                    {
                    });

                // Act
                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_using_textfile_reporter_with_setup_action_flush_interval_greater_than_zero_should_apply()
        {
            // Arrange
            var flushInterval = TimeSpan.FromSeconds(1);
            var builder = new MetricsBuilder().Report.ToTextFile(
                options =>
                {
                    options.OutputPathAndFileName = @".\metrics.txt";
                    options.FlushInterval = flushInterval;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
        }

        [Fact]
        public void When_using_textfile_reporter_with_setup_action_flush_interval_equal_to_zero_should_apply_default_interval()
        {
            // Arrange
            var flushInterval = TimeSpan.Zero;
            var builder = new MetricsBuilder().Report.ToTextFile(
                options =>
                {
                    options.OutputPathAndFileName = @".\metrics.txt";
                    options.FlushInterval = flushInterval;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
        }

        [Fact]
        public void When_using_textfile_reporter_with_setup_action_flush_interval_less_than_zero_should_throw()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var flushInterval = TimeSpan.FromSeconds(-1);
                var builder = new MetricsBuilder().Report.ToTextFile(
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
        public void Can_use_textfile_reporter_overriding_file_and_flush_interval()
        {
            // Arrange
            var fileName = @".\test.txt";
            var builder = new MetricsBuilder().Report.ToTextFile(fileName, TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();
            var reporter = metrics.Reporters.FirstOrDefault() as TextFileMetricsReporter;

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is TextFileMetricsReporter);
            reporter.Should().NotBeNull();
            reporter?.FlushInterval.Should().Be(TimeSpan.FromDays(1));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Textfile_reporter_should_throw_when_output_file_is_not_specified(string outputFile)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var builder = new MetricsBuilder().Report.ToTextFile(outputFile);
                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }
}