using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Health
{
    public class HealthCheckEndpointDisabledMiddlewareTests : IClassFixture<MetricsHostTestFixture<DisabledHealthCheckTestStartup>>
    {
        public HealthCheckEndpointDisabledMiddlewareTests(MetricsHostTestFixture<DisabledHealthCheckTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_disable_health_checks()
        {
            var result = await Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}