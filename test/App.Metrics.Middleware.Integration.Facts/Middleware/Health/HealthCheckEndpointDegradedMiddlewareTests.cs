// <copyright file="HealthCheckEndpointDegradedMiddlewareTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Middleware.Integration.Facts.Middleware.Health
{
    public class HealthCheckEndpointDegradedMiddlewareTests : IClassFixture<MetricsHostTestFixture<DegradedHealthTestStartup>>
    {
        public HealthCheckEndpointDegradedMiddlewareTests(MetricsHostTestFixture<DegradedHealthTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
            var result = await Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Headers.Warning.ToString().Should().StartWith("Warning: 100 ");
        }
    }
}