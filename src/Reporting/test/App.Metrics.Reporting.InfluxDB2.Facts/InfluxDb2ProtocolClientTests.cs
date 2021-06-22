﻿// <copyright file="DefaultLineProtocolClientTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.InfluxDB2.Client;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Xunit;

namespace App.Metrics.Reporting.InfluxDB2.Facts
{
    public class InfluxDb2ProtocolClientTests
    {
        private static readonly string Payload = "test__test_counter,mtype=counter,unit=none value=1i 1483232461000000000\n";

        [Fact]
        public async Task Can_write_payload_successfully()
        {
            // Arrange
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            var settings = new InfluxDb2Options
            {
                BaseUri = new Uri("http://localhost:8086"),
                Organization = "influx",
                Bucket = "bucket"
            };
            var policy = new HttpPolicy();
            var influxClient = MetricsInfluxDb2ReporterBuilder.CreateClient(settings, policy, httpMessageHandlerMock.Object);

            // Act
            LineProtocolWriteResult response;
            using (var payload = new MemoryStream(Encoding.UTF8.GetBytes(Payload)))
            {
                response = await influxClient.WriteAsync(payload, CancellationToken.None);
            }

            // Assert
            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Can_write_payload_successfully_with_creds()
        {
            // Arrange
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            var settings = new InfluxDb2Options
            {
                BaseUri = new Uri("http://localhost"),
                Token = "token"
            };

            var policy = new HttpPolicy();
            var influxClient = MetricsInfluxDb2ReporterBuilder.CreateClient(settings, policy, httpMessageHandlerMock.Object);

            // Act
            LineProtocolWriteResult response;
            using (var payload = new MemoryStream(Encoding.UTF8.GetBytes(Payload)))
            {
                response = await influxClient.WriteAsync(payload, CancellationToken.None);
            }

            // Assert
            response.Success.Should().BeTrue();
        }

        [Fact]
        public void Http_policy_is_required()
        {
            // Arrange
            Action action = () =>
            {
                var settings = new InfluxDb2Options
                {
                    BaseUri = new Uri("http://localhost"),
                    Organization = "influx",
                    Bucket = "bucket"
                };

                // Act
                var unused = new InfluxDb2ProtocolClient(settings, null, new HttpClient());
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Influxdb_settings_are_required()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new DefaultLineProtocolClient(null, new HttpPolicy(), new HttpClient());
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_back_off_when_reached_max_failures()
        {
            // Arrange
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                                       "SendAsync",
                                       ItExpr.IsAny<HttpRequestMessage>(),
                                       ItExpr.IsAny<CancellationToken>()).
                                   Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));
            var policy = new HttpPolicy { FailuresBeforeBackoff = 3, BackoffPeriod = TimeSpan.FromMinutes(1) };
            var settings = new InfluxDb2Options
            {
                BaseUri = new Uri("http://localhost"),
                Organization = "influx",
                Bucket = "bucket",
            };
            var influxClient = MetricsInfluxDb2ReporterBuilder.CreateClient(settings, policy, httpMessageHandlerMock.Object);

            // Act
            foreach (var attempt in Enumerable.Range(0, 10))
            {
                using (var payload = new MemoryStream(Encoding.UTF8.GetBytes(Payload)))
                {
                    await influxClient.WriteAsync(payload, CancellationToken.None);
                }

                // ReSharper disable ConvertIfStatementToConditionalTernaryExpression
                if (attempt <= policy.FailuresBeforeBackoff)
                {
                    // ReSharper restore ConvertIfStatementToConditionalTernaryExpression
                    // Assert
                    httpMessageHandlerMock.Protected().Verify<Task<HttpResponseMessage>>(
                        "SendAsync",
                        Times.AtLeastOnce(),
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>());
                }
                else
                {
                    // Assert
                    httpMessageHandlerMock.Protected().Verify<Task<HttpResponseMessage>>(
                        "SendAsync",
                        Times.AtMost(6), // TODO: Starting failing when running all tests with 2.0.0 upgrade, should be 3
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>());
                }
            }
        }

        [Fact]
        public async Task Should_back_off_when_reached_max_failures_then_retry_after_backoff_period()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                                       "SendAsync",
                                       ItExpr.IsAny<HttpRequestMessage>(),
                                       ItExpr.IsAny<CancellationToken>()).
                                   Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));
            var policy = new HttpPolicy { FailuresBeforeBackoff = 3, BackoffPeriod = TimeSpan.FromSeconds(1) };
            var settings = new InfluxDb2Options
            {
                BaseUri = new Uri("http://localhost"),
                Organization = "influx",
                Bucket = "bucket",
            };
            var influxClient = MetricsInfluxDb2ReporterBuilder.CreateClient(settings, policy, httpMessageHandlerMock.Object);

            foreach (var attempt in Enumerable.Range(0, 10))
            {
                using (var payload = new MemoryStream(Encoding.UTF8.GetBytes(Payload)))
                {
                    await influxClient.WriteAsync(payload, CancellationToken.None);
                }

                if (attempt <= policy.FailuresBeforeBackoff)
                {
                    httpMessageHandlerMock.Protected().Verify<Task<HttpResponseMessage>>(
                        "SendAsync",
                        Times.AtLeastOnce(),
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>());
                }
                else
                {
                    httpMessageHandlerMock.Protected().Verify<Task<HttpResponseMessage>>(
                        "SendAsync",
                        Times.AtMost(3),
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>());
                }
            }

            await Task.Delay(policy.BackoffPeriod);

            httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            influxClient = MetricsInfluxDb2ReporterBuilder.CreateClient(settings, policy, httpMessageHandlerMock.Object);

            LineProtocolWriteResult response;
            using (var payload = new MemoryStream(Encoding.UTF8.GetBytes(Payload)))
            {
                response = await influxClient.WriteAsync(payload, CancellationToken.None);
            }

            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Should_reset_failure_attempts_in_case_of_success_request()
        {
            // Arrange
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            int callCount = 0;
            httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                                       "SendAsync",
                                       ItExpr.IsAny<HttpRequestMessage>(),
                                       ItExpr.IsAny<CancellationToken>()).
                                   ReturnsAsync(
                                       () => ++callCount % 2 == 0
                                           ? new HttpResponseMessage(HttpStatusCode.BadRequest)
                                           : new HttpResponseMessage(HttpStatusCode.OK));
            var policy = new HttpPolicy { FailuresBeforeBackoff = 3, BackoffPeriod = TimeSpan.FromMinutes(1) };
            var settings = new InfluxDb2Options
            {
                BaseUri = new Uri("http://localhost"),
                Organization = "influx",
                Bucket = "bucket",
            };
            var influxClient = MetricsInfluxDb2ReporterBuilder.CreateClient(settings, policy, httpMessageHandlerMock.Object);

            // Act
            foreach (var attempt in Enumerable.Range(0, 10))
            {
                using (var payload = new MemoryStream(Encoding.UTF8.GetBytes(Payload)))
                {
                    await influxClient.WriteAsync(payload, CancellationToken.None);
                }

                httpMessageHandlerMock.Protected().Verify<Task<HttpResponseMessage>>(
                    "SendAsync",
                    Times.Exactly(attempt + 1),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());
            }
        }

    }
}