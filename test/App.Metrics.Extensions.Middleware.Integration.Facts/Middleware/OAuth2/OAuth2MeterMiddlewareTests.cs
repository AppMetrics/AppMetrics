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

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.OAuth2
{
    public class OAuth2MeterMiddlewareTests : IClassFixture<MetricsHostTestFixture<OAuthTestStartup>>
    {
        public OAuth2MeterMiddlewareTests(MetricsHostTestFixture<OAuthTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_track_status_codes_per_client_when_oauth2_tracking_enabled()
        {
            await Client.GetAsync("/api/test/oauth/client1");
            await Client.GetAsync("/api/test/oauth/client1");
            await Client.GetAsync("/api/test/oauth/error/client1");
            await Client.GetAsync("/api/test/oauth/error/client2");
            await Client.GetAsync("/api/test/oauth/error/client2");

            Func<string, MeterValue> getMeterValue = metricName => Context.Snapshot.GetMeterValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);
            Func<string, TimerValue> getTimerValue = metricName => Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            var errorMeterClient1 = getMeterValue(
                "Http Error Requests|route:GET api/test/oauth/error/{clientid},http_status_code:500,client_id:client1");
            var errorTimerClient1 = getTimerValue("Http Request Transactions|route:GET api/test/oauth/error/{clientid},client_id:client1");

            errorMeterClient1.Count.Should().Be(1);
            errorTimerClient1.Histogram.Count.Should().Be(1);

            var errorMeterClient2 = getMeterValue(
                "Http Error Requests|route:GET api/test/oauth/error/{clientid},http_status_code:500,client_id:client2");
            var errorTimerClient2 = getTimerValue("Http Request Transactions|route:GET api/test/oauth/error/{clientid},client_id:client2");

            errorMeterClient2.Count.Should().Be(2);
            errorTimerClient2.Histogram.Count.Should().Be(2);

            var okTimerClient1 = getTimerValue("Http Request Transactions|route:GET api/test/oauth/{clientid},client_id:client1");

            okTimerClient1.Histogram.Count.Should().Be(2);
        }
    }
}