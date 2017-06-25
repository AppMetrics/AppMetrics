// <copyright file="PingEndpointDisabledMiddlewareTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.Ping
{
    public class PingEndpointDisabledMiddlewareTests : IClassFixture<MetricsHostTestFixture<PingDisabledTestStartup>>
    {
        public PingEndpointDisabledMiddlewareTests(MetricsHostTestFixture<PingDisabledTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task When_enabled_returns_pong()
        {
            var result = await Client.GetAsync("/ping");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}