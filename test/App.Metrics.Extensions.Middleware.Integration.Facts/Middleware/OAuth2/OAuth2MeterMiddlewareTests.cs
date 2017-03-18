// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Histogram;
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
            await Client.GetAsync("/api/test/oauth/client2");
            await Client.GetAsync("/api/test/oauth/error/client1");
            await Client.GetAsync("/api/test/oauth/error/client2");
            await Client.GetAsync("/api/test/oauth/error/client2");

            MeterValue GetMeterValue(string metricName) => Context.Snapshot.GetMeterValue(
                OAuthRequestMetricsRegistry.ContextName,
                metricName);

            var meterClient1 = GetMeterValue(
                "Request Rate|client_id:client1,route:GET api/test/oauth/{clientid}");
            meterClient1.Count.Should().Be(2);

            var errorMeterClient1 = GetMeterValue(
                "Error Rate|client_id:client1,route:GET api/test/oauth/error/{clientid},http_status_code:500");
            errorMeterClient1.Count.Should().Be(1);

            var meterClient2 = GetMeterValue(
                "Request Rate|client_id:client2,route:GET api/test/oauth/{clientid}");
            meterClient2.Count.Should().Be(1);

            var errorMeterClient2 = GetMeterValue(
                "Error Rate|client_id:client2,route:GET api/test/oauth/error/{clientid},http_status_code:500");
            errorMeterClient2.Count.Should().Be(2);
        }
    }    
}