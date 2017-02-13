// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Meter;
using App.Metrics.Meter.Extensions;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Metrics
{
    public class ErrorRequestMeterMiddlewarePerEndPointTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorRequestMeterMiddlewarePerEndPointTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/unauth");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/error");
            await Client.GetAsync("/api/test/error");

            Func<string, MeterValue> getMeterValue = metricName => Context.Snapshot.GetMeterValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            Func<string, TimerValue> getTimerValue = metricName => Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            getMeterValue("GET api/test/bad").Count.Should().Be(1);
            getMeterValue("GET api/test/error").Count.Should().Be(2);
            getMeterValue("GET api/test/unauth").Count.Should().Be(1);
            getMeterValue("Http Error Requests").Count.Should().Be(4);
            getMeterValue("Http Error Requests").
                Items.Length.Should().
                Be(3, "there are three endpoints which had an unsuccessful status code");
            getMeterValue("GET api/test/bad").Count.Should().Be(1);
            getMeterValue("Http Error Requests").Count.Should().Be(4);
            getTimerValue("Http Requests").Histogram.Count.Should().Be(5);
        }
    }
}