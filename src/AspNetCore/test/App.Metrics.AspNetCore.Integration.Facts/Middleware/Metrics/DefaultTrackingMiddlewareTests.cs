// <copyright file="DefaultTrackingMiddlewareTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.Metrics
{
    public class DefaultTrackingMiddlewareTests : IClassFixture<MetricsHostTestFixture<DefaultTrackingEnabledOffTestStartup>>
    {
        public DefaultTrackingMiddlewareTests(MetricsHostTestFixture<DefaultTrackingEnabledOffTestStartup> fixture) { Client = fixture.Client; }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_disable_registring_off_default_tracking_middleware()
        {
            var unauthorizedResponse = await Client.GetAsync("/api/test/401");
            unauthorizedResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var badResponse = await Client.GetAsync("/api/test/400");
            badResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await Client.GetAsync("/metrics");
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var stringResult = await result.Content.ReadAsStringAsync();
            stringResult.Should().NotContain("GET api/test/401");
            stringResult.Should().NotContain("GET api/test/400");
        }
    }
}