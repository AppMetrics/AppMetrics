// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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

        private HttpClient Client { get; }

        private IMetrics Context { get; }

        [Fact]
        public async Task can_track_status_codes_per_client_when_oauth2_tracking_enabled()
        {
            await Client.GetAsync("/api/test/oauth/client1");
            await Client.GetAsync("/api/test/oauth/client1");
            await Client.GetAsync("/api/test/oauth/error/client1");
            await Client.GetAsync("/api/test/oauth/error/client2");
            await Client.GetAsync("/api/test/oauth/error/client2");

            MeterValue GetMeterValue(string metricName) => Context.Snapshot.GetMeterValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            TimerValue GetTimerValue(string metricName) => Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            var errorMeterClient1 = GetMeterValue(
                "Error Rate Per Endpoint And Status Code|route:GET api/test/oauth/error/{clientid},http_status_code:500,client_id:client1");
            var errorTimerClient1 = GetTimerValue("Transactions Per Endpoint|route:GET api/test/oauth/error/{clientid},client_id:client1");

            errorMeterClient1.Count.Should().Be(1);
            errorTimerClient1.Histogram.Count.Should().Be(1);

            var errorMeterClient2 = GetMeterValue(
                "Error Rate Per Endpoint And Status Code|route:GET api/test/oauth/error/{clientid},http_status_code:500,client_id:client2");
            var errorTimerClient2 = GetTimerValue("Transactions Per Endpoint|route:GET api/test/oauth/error/{clientid},client_id:client2");

            errorMeterClient2.Count.Should().Be(2);
            errorTimerClient2.Histogram.Count.Should().Be(2);

            var okTimerClient1 = GetTimerValue("Transactions Per Endpoint|route:GET api/test/oauth/{clientid},client_id:client1");

            okTimerClient1.Histogram.Count.Should().Be(2);
        }
    }
}