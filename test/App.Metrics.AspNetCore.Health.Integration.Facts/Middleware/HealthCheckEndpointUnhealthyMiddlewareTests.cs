// <copyright file="HealthCheckEndpointUnhealthyMiddlewareTests.cs" company="Allan Hardy">
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
    public class HealthCheckEndpointUnhealthyMiddlewareTests : IClassFixture<StartupTestFixture<UnhealthyHealthTestStartup>>
    {
        public HealthCheckEndpointUnhealthyMiddlewareTests(StartupTestFixture<UnhealthyHealthTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
            var result = await Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        }
    }
}