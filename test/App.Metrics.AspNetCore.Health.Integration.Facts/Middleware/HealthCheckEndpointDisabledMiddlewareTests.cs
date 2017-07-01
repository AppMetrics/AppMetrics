// <copyright file="HealthCheckEndpointDisabledMiddlewareTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Health.Integration.Facts.Middleware
{
    public class HealthCheckEndpointDisabledMiddlewareTests : IClassFixture<StartupTestFixture<DisabledHealthCheckTestStartup>>
    {
        public HealthCheckEndpointDisabledMiddlewareTests(StartupTestFixture<DisabledHealthCheckTestStartup> fixture) { Client = fixture.Client; }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_disable_health_checks()
        {
            var result = await Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
