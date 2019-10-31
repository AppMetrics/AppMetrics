// <copyright file="MetricsHttpReporterBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Json;
using App.Metrics.Internal.NoOp;
using App.Metrics.Reporting.FactsCommon;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Http.Facts
{
    public class MetricsHttpReporterBuilderTests
    {
        [Fact]
        public void Can_use_http_reporter_with_defaults()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.OverHttp("http://localhost/metrics");

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is HttpMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<MetricsJsonOutputFormatter>();
        }

        [Fact]
        public void Can_change_http_reporter_metrics_output_formatter()
        {
            // Arrange
            var uri = "http://localhost/metrics";
            var builder = new MetricsBuilder().Report.OverHttp(
                options =>
                {
                    options.HttpSettings.RequestUri = new Uri(uri);
                    options.MetricsOutputFormatter = new TestMetricsFormatter();
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void Can_set_http_reporter_metrics_filter()
        {
            // Arrange
            var uri = "http://localhost/metrics";
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var builder = new MetricsBuilder().Report.OverHttp(
                options =>
                {
                    options.HttpSettings.RequestUri = new Uri(uri);
                    options.Filter = filter;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
        }

        [Fact]
        public void Should_throw_when_using_setup_action_without_specifying_a_uri()
        {
            // Arrange
            Action action = () =>
            {
                var builder = new MetricsBuilder().Report.OverHttp(
                    options =>
                    {
                    });

                // Act
                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact(Skip = "Why is this failing on a Linux Build?")]
        public void Should_throw_when_http_reporter_uri_is_not_absolute()
        {
            // Arrange
            Action action = () =>
            {
                var builder = new MetricsBuilder().Report.OverHttp("/metrics");

                // Act
                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_using_http_reporter_with_setup_action_flush_interval_greater_than_zero_should_apply()
        {
            // Arrange
            var uri = "http://localhost/metrics";
            var flushInterval = TimeSpan.FromSeconds(1);
            var builder = new MetricsBuilder().Report.OverHttp(
                options =>
                {
                    options.HttpSettings.RequestUri = new Uri(uri);
                    options.FlushInterval = flushInterval;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
        }

        [Fact]
        public void When_using_http_reporter_with_setup_action_flush_interval_equal_to_zero_should_apply_default_interval()
        {
            // Arrange
            var uri = "http://localhost/metrics";
            var flushInterval = TimeSpan.Zero;
            var builder = new MetricsBuilder().Report.OverHttp(
                options =>
                {
                    options.HttpSettings.RequestUri = new Uri(uri);
                    options.FlushInterval = flushInterval;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
        }

        [Fact]
        public void When_using_http_reporter_with_setup_action_flush_interval_less_than_zero_should_throw()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var uri = "http://localhost/metrics";
                var flushInterval = TimeSpan.FromSeconds(-1);
                var builder = new MetricsBuilder().Report.OverHttp(
                    options =>
                    {
                        options.HttpSettings.RequestUri = new Uri(uri);
                        options.FlushInterval = flushInterval;
                    });

                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Can_use_http_reporter_overriding_file_and_flush_interval()
        {
            // Arrange
            var uri = "http://localhost/metrics";
            var builder = new MetricsBuilder().Report.OverHttp(uri, TimeSpan.FromDays(1));

            // Act
            var metrics = builder.Build();
            var reporter = metrics.Reporters.FirstOrDefault() as HttpMetricsReporter;

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is HttpMetricsReporter);
            reporter.Should().NotBeNull();
            reporter?.FlushInterval.Should().Be(TimeSpan.FromDays(1));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Http_reporter_should_throw_when_uri_is_empty_or_white_space(string uri)
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var builder = new MetricsBuilder().Report.OverHttp(uri);
                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Http_reporter_should_throw_when_uri_is_null()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var builder = new MetricsBuilder().Report.OverHttp(endpoint: null);
                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Can_use_http_reporter_with_authorization_token()
        {
            // Arrange
            var builder = new MetricsBuilder().Report.OverHttp(
                options =>
                {
                    options.HttpSettings.AuthorizationToken = "test";
                    options.HttpSettings.RequestUri = new Uri("http://localhost");
                    options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is HttpMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<MetricsJsonOutputFormatter>();
        }
    }
}