// <copyright file="SocketMetricsReporterTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filtering;
using App.Metrics.Reporting.FactsCommon;
using App.Metrics.Reporting.Socket.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Socket.Facts
{
    public class SocketMetricsReporterTests
    {
        private static ProtocolType defaultProtocol = ProtocolType.Udp;
        private static string defaultAddress = "localhost";
        private static int defaultPort = 8094;

        [Fact]
        public void Socket_metrics_reporter_expect_options()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var reporter = new SocketMetricsReporter(null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Socket_metrics_reporter_expect_output_formatter()
        {
            // Arrange
            Action action = () =>
            {
                var options = new MetricsReportingSocketOptions();

                // Act
                var reporter = new SocketMetricsReporter(options);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Socket_metrics_reporter_expect_socket_settings()
        {
            // Arrange
            Action action = () =>
            {
                var formatter = new TestMetricsFormatter();
                var options = new MetricsReportingSocketOptions
                                {
                                    MetricsOutputFormatter = formatter
                                };
                // Act
                var reporter = new SocketMetricsReporter(options);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Socket_settings_expect_supported_protocol()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var settings = new SocketSettings(ProtocolType.Ipx, defaultAddress, defaultPort);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Socket_settings_expect_non_empty_address()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var settings = new SocketSettings(defaultProtocol, string.Empty, defaultPort);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Socket_settings_expect_port_greater_than_zero()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var settings = new SocketSettings(defaultProtocol, defaultAddress, 0);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Socket_settings_expect_port_less_than_maxium_available_port()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var settings = new SocketSettings(defaultProtocol, defaultAddress, 97531);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Socket_settings_expect_using_unix_domain_sockets_with_null_port()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var settings = new SocketSettings(ProtocolType.IP, defaultAddress, defaultPort);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task Can_flush_metrics_via_sockets_with_options()
        {
            // Arrange
            var filter = new MetricsFilter().WhereType(MetricType.Apdex);
            var formatter = new TestMetricsFormatter();
            var interval = TimeSpan.FromDays(1);
            var settings = new SocketSettings(defaultProtocol, defaultAddress, defaultPort);
            var options = new MetricsReportingSocketOptions
                            {
                                Filter = filter,
                                FlushInterval = interval,
                                MetricsOutputFormatter = formatter,
                                SocketSettings = settings
                            };
            var reporter = new SocketMetricsReporter(options);
            var snapshot = new MetricsDataValueSource(DateTime.Now, Enumerable.Empty<MetricsContextValueSource>());

            // Act
            var result = await reporter.FlushAsync(snapshot, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }
    }
}
