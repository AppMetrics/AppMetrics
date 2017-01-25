// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Tagging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Client
{
    public class LineProtocolClientTests
    {
        private readonly LineProtocolPayload _payload;

        public LineProtocolClientTests()
        {
            _payload = new LineProtocolPayload();
            var fieldsOne = new Dictionary<string, object> { { "key", "value" } };
            var timestampOne = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var pointOne = new LineProtocolPoint("measurement", fieldsOne, MetricTags.None, timestampOne);
            _payload.Add(pointOne);
        }

        [Fact]
        public async Task can_write_payload_successfully()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<CancellationToken>())
                                  .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            var client = new DefaultLineProtocolClient(
                new LoggerFactory(),
                new InfluxDBSettings("influx", new Uri("http://localhost")),
                new HttpPolicy(),
                httpMessageHandlerMock.Object);

            var response = await client.WriteAsync(_payload, CancellationToken.None);

            response.Success.Should().BeTrue();
        }

        [Fact]
        public async Task can_write_payload_successfully_with_creds()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<CancellationToken>())
                                  .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            var client = new DefaultLineProtocolClient(
                new LoggerFactory(),
                new InfluxDBSettings("influx", new Uri("http://localhost"))
                {
                    UserName = "admin",
                    Password = "password"
                },
                new HttpPolicy(),
                httpMessageHandlerMock.Object);

            var response = await client.WriteAsync(_payload, CancellationToken.None);

            response.Success.Should().BeTrue();
        }

        [Fact]
        public void databse_is_required()
        {
            Action action = () =>
            {
                var client = new DefaultLineProtocolClient(
                    new LoggerFactory(),
                    new InfluxDBSettings(null, new Uri("http://localhost")),
                    new HttpPolicy());
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void http_policy_is_required()
        {
            Action action = () =>
            {
                var client = new DefaultLineProtocolClient(new LoggerFactory(), new InfluxDBSettings("influx", new Uri("http://localhost")), null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void influxdb_settings_are_required()
        {
            Action action = () =>
            {
                var client = new DefaultLineProtocolClient(new LoggerFactory(), null, new HttpPolicy());
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task should_back_off_when_reached_max_failures()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<CancellationToken>())
                                  .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));
            var policy = new HttpPolicy { FailuresBeforeBackoff = 3 };

            var client = new DefaultLineProtocolClient(
                new LoggerFactory(),
                new InfluxDBSettings("influx", new Uri("http://localhost")),
                policy,
                httpMessageHandlerMock.Object);

            foreach (var attempt in Enumerable.Range(0, 10))
            {
                await client.WriteAsync(_payload, CancellationToken.None);

                // ReSharper disable ConvertIfStatementToConditionalTernaryExpression
                if (attempt <= policy.FailuresBeforeBackoff)
                    // ReSharper restore ConvertIfStatementToConditionalTernaryExpression
                {
                    httpMessageHandlerMock.Protected()
                                          .Verify<Task<HttpResponseMessage>>(
                                              "SendAsync",
                                              Times.AtLeastOnce(),
                                              ItExpr.IsAny<HttpRequestMessage>(),
                                              ItExpr.IsAny<CancellationToken>());
                }
                else
                {
                    httpMessageHandlerMock.Protected()
                                          .Verify<Task<HttpResponseMessage>>(
                                              "SendAsync",
                                              Times.AtMost(3),
                                              ItExpr.IsAny<HttpRequestMessage>(),
                                              ItExpr.IsAny<CancellationToken>());
                }
            }
        }

        [Fact]
        public async Task should_back_off_when_reached_max_failures_then_retry_after_backoff_period()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<CancellationToken>())
                                  .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));
            var policy = new HttpPolicy { FailuresBeforeBackoff = 3, BackoffPeriod = TimeSpan.FromSeconds(1) };

            var client = new DefaultLineProtocolClient(
                new LoggerFactory(),
                new InfluxDBSettings("influx", new Uri("http://localhost")),
                policy,
                httpMessageHandlerMock.Object);

            foreach (var attempt in Enumerable.Range(0, 10))
            {
                await client.WriteAsync(_payload, CancellationToken.None);

                if (attempt <= policy.FailuresBeforeBackoff)
                {
                    httpMessageHandlerMock.Protected()
                                          .Verify<Task<HttpResponseMessage>>(
                                              "SendAsync",
                                              Times.AtLeastOnce(),
                                              ItExpr.IsAny<HttpRequestMessage>(),
                                              ItExpr.IsAny<CancellationToken>());
                }
                else
                {
                    httpMessageHandlerMock.Protected()
                                          .Verify<Task<HttpResponseMessage>>(
                                              "SendAsync",
                                              Times.AtMost(3),
                                              ItExpr.IsAny<HttpRequestMessage>(),
                                              ItExpr.IsAny<CancellationToken>());
                }
            }

            await Task.Delay(policy.BackoffPeriod);

            httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock.Protected()
                                  .Setup<Task<HttpResponseMessage>>(
                                      "SendAsync",
                                      ItExpr.IsAny<HttpRequestMessage>(),
                                      ItExpr.IsAny<CancellationToken>())
                                  .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            client = new DefaultLineProtocolClient(
                new LoggerFactory(),
                new InfluxDBSettings("influx", new Uri("http://localhost")),
                policy,
                httpMessageHandlerMock.Object);

            var response = await client.WriteAsync(_payload, CancellationToken.None);

            response.Success.Should().BeTrue();
        }
    }
}