// <copyright file="MetricsCustomEndpointMiddlewareTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.Metrics
{
    public class MetricsCustomEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<CustomMetricsEndpointTestStartup>>
    {
        public MetricsCustomEndpointMiddlewareTests(MetricsHostTestFixture<CustomMetricsEndpointTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_change_metrics_endpoint()
        {
            var result = await Client.GetAsync("/metrics");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result = await Client.GetAsync("/metrics-json");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}