using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Health
{
    public class HealthCheckEndpointUnhealthyMiddlewareTests : IClassFixture<MetricsHostTestFixture<UnhealthyHealthTestStartup>>
    {
        public HealthCheckEndpointUnhealthyMiddlewareTests(MetricsHostTestFixture<UnhealthyHealthTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
            var result = await Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}