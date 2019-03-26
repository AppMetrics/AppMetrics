// <copyright file="MetricsEndpointIgnoreRouteMiddlewareTests.cs" company="App Metrics Contributors">
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
    public class MetricsEndpointIgnoreRouteMiddlewareTests : IClassFixture<MetricsHostTestFixture<IgnoredRouteTestStartup>>
    {
        public MetricsEndpointIgnoreRouteMiddlewareTests(MetricsHostTestFixture<IgnoredRouteTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_ignore_specified_routes()
        {
            var ignoreResult = await Client.GetAsync("/api/test/ignore");
            ignoreResult.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await Client.GetAsync("/metrics");
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var stringResult = await result.Content.ReadAsStringAsync();
            stringResult.Should().NotContain("GET api/test/ignore");
        }
    }
}