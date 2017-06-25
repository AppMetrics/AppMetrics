// <copyright file="PingEndpointMiddlewareTests.cs" company="Allan Hardy">
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
    public class PingEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<PingTestStartup>>
    {
        public PingEndpointMiddlewareTests(MetricsHostTestFixture<PingTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Resposne_is_plain_text_content_type()
        {
            var result = await Client.GetAsync("/ping");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "text/plain");
        }

        [Fact]
        public async Task Returns_correct_response_headers()
        {
            var result = await Client.GetAsync("/ping");

            result.Headers.CacheControl.NoCache.Should().Be(true);
            result.Headers.CacheControl.NoStore.Should().Be(true);
            result.Headers.CacheControl.MustRevalidate.Should().Be(true);
            result.Headers.Pragma.ToString().Should().Be("no-cache");
        }

        [Fact]
        public async Task When_enabled_returns_pong()
        {
            var result = await Client.GetAsync("/ping");
            var response = await result.Content.ReadAsStringAsync();

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().Be("pong");
        }
    }
}