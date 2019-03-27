// <copyright file="MetricsTextEndpointDisabledMiddlewareTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.MetricsText
{
    public class MetricsTextEndpointDisabledMiddlewareTests : IClassFixture<MetricsHostTestFixture<DisabledTextEndpointTestStartup>>
    {
        public MetricsTextEndpointDisabledMiddlewareTests(MetricsHostTestFixture<DisabledTextEndpointTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_disable_metrics_text_endpoint_when_metrics_enabled()
        {
            var result = await Client.GetAsync("/metrics-text");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}