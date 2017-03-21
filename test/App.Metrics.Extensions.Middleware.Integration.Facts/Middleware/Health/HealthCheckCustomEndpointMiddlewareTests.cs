using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Health
{
    public class HealthCheckCustomEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<CustomHealthCheckTestStartup>>
    {
        public HealthCheckCustomEndpointMiddlewareTests(MetricsHostTestFixture<CustomHealthCheckTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task can_change_health_endpoint()
        {
            var result = await Client.GetAsync("/health");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result = await Client.GetAsync("/health-status");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}