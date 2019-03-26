// <copyright file="MetricsSocketReporterBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Net.Sockets;
using App.Metrics.Filtering;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Internal.NoOp;
using App.Metrics.Reporting.FactsCommon;
using App.Metrics.Reporting.Socket.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Socket.Facts
{
    public class MetricsSocketReporterBuilderTests
    {
        private static ProtocolType defaultProtocol = ProtocolType.Udp;
        private static string defaultAddress = "localhost";
        private static string defaultUnixAddress = "//path/to/socket.file";
        private static int defaultPort = 8094;

        [Fact]
        public void Can_use_tcp_socket_reporter()
        {
            // Arrange
            var formatter = new TestMetricsFormatter();
            var builder = new MetricsBuilder().Report.OverTcp(formatter, defaultAddress, defaultPort);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void Can_use_udp_socket_reporter()
        {
            // Arrange
            var formatter = new TestMetricsFormatter();
            var builder = new MetricsBuilder().Report.OverUdp(formatter, defaultAddress, defaultPort);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact(Skip = "Test should work only on unix platform")]
        public void Can_use_uds_socket_reporter()
        {
            // Arrange
            var formatter = new TestMetricsFormatter();
            var builder = new MetricsBuilder().Report.OverUds(formatter, defaultUnixAddress);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
            metrics.Reporters.First().Filter.Should().BeOfType<NullMetricsFilter>();
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void Cannot_use_tcp_socket_reporter_without_formatter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var builder = new MetricsBuilder().Report.OverTcp(null, defaultAddress, defaultPort);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Cannot_use_udp_socket_reporter_without_formatter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var builder = new MetricsBuilder().Report.OverUdp(null, defaultAddress, defaultPort);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Cannot_use_uds_socket_reporter_without_formatter()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var builder = new MetricsBuilder().Report.OverUds(null, defaultUnixAddress);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Can_use_tcp_socket_reporter_with_options()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var flushInterval = TimeSpan.FromDays(1);
            var settings = new SocketSettings(defaultProtocol, defaultAddress, defaultPort);
            var options = new MetricsReportingSocketOptions();
            options.Filter = filter;
            options.FlushInterval = flushInterval;
            options.MetricsOutputFormatter = new TestMetricsFormatter();
            options.SocketSettings = settings;
            var builder = new MetricsBuilder().Report.OverTcp(options);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void Can_use_udp_socket_reporter_with_options()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var flushInterval = TimeSpan.FromDays(1);
            var settings = new SocketSettings(defaultProtocol, defaultAddress, defaultPort);
            var options = new MetricsReportingSocketOptions();
            options.Filter = filter;
            options.FlushInterval = flushInterval;
            options.MetricsOutputFormatter = new TestMetricsFormatter();
            options.SocketSettings = settings;
            var builder = new MetricsBuilder().Report.OverUdp(options);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact(Skip = "Test should work only on unix platform")]
        public void Can_use_uds_socket_reporter_with_options()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var flushInterval = TimeSpan.FromDays(1);
            var settings = new SocketSettings();
            settings.Address = defaultUnixAddress;
            var options = new MetricsReportingSocketOptions();
            options.Filter = filter;
            options.FlushInterval = flushInterval;
            options.MetricsOutputFormatter = new TestMetricsFormatter();
            options.SocketSettings = settings;
            var builder = new MetricsBuilder().Report.OverUds(options);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void Can_use_tcp_socket_reporter_with_setup_actions()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var flushInterval = TimeSpan.FromDays(1);
            var settings = new SocketSettings(defaultProtocol, defaultAddress, defaultPort);
            var builder = new MetricsBuilder().Report.OverTcp(
                options =>
                {
                    options.Filter = filter;
                    options.FlushInterval = flushInterval;
                    options.MetricsOutputFormatter = new TestMetricsFormatter();
                    options.SocketSettings = settings;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void Can_use_udp_socket_reporter_with_setup_actions()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var flushInterval = TimeSpan.FromDays(1);
            var settings = new SocketSettings(defaultProtocol, defaultAddress, defaultPort);
            var builder = new MetricsBuilder().Report.OverUdp(
                options =>
                {
                    options.Filter = filter;
                    options.FlushInterval = flushInterval;
                    options.MetricsOutputFormatter = new TestMetricsFormatter();
                    options.SocketSettings = settings;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact(Skip = "Test should work only on unix platform")]
        public void Can_use_uds_socket_reporter_with_setup_actions()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var flushInterval = TimeSpan.FromDays(1);
            var settings = new SocketSettings();
            settings.Address = defaultUnixAddress;
            var builder = new MetricsBuilder().Report.OverUds(
                options =>
                {
                    options.Filter = filter;
                    options.FlushInterval = flushInterval;
                    options.MetricsOutputFormatter = new TestMetricsFormatter();
                    options.SocketSettings = settings;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
            metrics.Reporters.First().Filter.Should().BeSameAs(filter);
            metrics.Reporters.First().Formatter.Should().BeOfType<TestMetricsFormatter>();
        }

        [Fact]
        public void When_using_socket_reporter_with_setu_actions_flush_interval_equal_to_zero_should_apply_default_interval()
        {
            // Arrange
            var flushInterval = TimeSpan.Zero;
            var settings = new SocketSettings(defaultProtocol, defaultAddress, defaultPort);
            var builder = new MetricsBuilder().Report.OverTcp(
                options =>
                {
                    options.FlushInterval = flushInterval;
                    options.MetricsOutputFormatter = new TestMetricsFormatter();
                    options.SocketSettings = settings;
                });

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.First().FlushInterval.Should().Be(AppMetricsConstants.Reporting.DefaultFlushInterval);
        }

        [Fact]
        public void When_using_socket_reporter_with_setup_actions_flush_interval_less_than_zero_should_throw()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var flushInterval = TimeSpan.FromSeconds(-1);
                var settings = new SocketSettings(defaultProtocol, defaultAddress, defaultPort);
                var builder = new MetricsBuilder().Report.OverTcp(
                    options =>
                    {
                        options.FlushInterval = flushInterval;
                        options.MetricsOutputFormatter = new TestMetricsFormatter();
                        options.SocketSettings = settings;
                    });

                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Can_use_tcp_socket_reporter_overriding_flush_interval()
        {
            // Arrange
            var formatter = new TestMetricsFormatter();
            var flushInterval = TimeSpan.FromDays(1);
            var builder = new MetricsBuilder().Report.OverTcp(formatter, defaultAddress, defaultPort, flushInterval);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
        }

        [Fact]
        public void Can_use_udp_socket_reporter_overriding_flush_interval()
        {
            // Arrange
            var formatter = new TestMetricsFormatter();
            var flushInterval = TimeSpan.FromDays(1);
            var builder = new MetricsBuilder().Report.OverUdp(formatter, defaultAddress, defaultPort, flushInterval);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
        }

        [Fact(Skip = "Test should work only on unix platform")]
        public void Can_use_uds_socket_reporter_overriding_flush_interval()
        {
            // Arrange
            var formatter = new TestMetricsFormatter();
            var flushInterval = TimeSpan.FromDays(1);
            var builder = new MetricsBuilder().Report.OverUds(formatter, defaultAddress, flushInterval);

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Reporters.Should().Contain(reportMetrics => reportMetrics is SocketMetricsReporter);
            metrics.Reporters.First().FlushInterval.Should().Be(flushInterval);
        }
    }
}
