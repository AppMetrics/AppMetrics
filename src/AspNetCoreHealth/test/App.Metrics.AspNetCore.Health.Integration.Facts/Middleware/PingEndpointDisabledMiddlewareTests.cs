// <copyright file="PingEndpointDisabledMiddlewareTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Middleware
{
    public class PingEndpointDisabledMiddlewareTests : IClassFixture<StartupTestFixture<PingDisabledTestStartup>>
    {
        public PingEndpointDisabledMiddlewareTests(StartupTestFixture<PingDisabledTestStartup> fixture)
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