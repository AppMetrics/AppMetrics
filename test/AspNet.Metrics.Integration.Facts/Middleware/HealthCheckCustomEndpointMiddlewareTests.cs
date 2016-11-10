using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Integration.Facts.Middleware
{
    public class HealthCheckCustomEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<CustomHealthCheckTestStartup>>
    {
        public HealthCheckCustomEndpointMiddlewareTests(MetricsHostTestFixture<CustomHealthCheckTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

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