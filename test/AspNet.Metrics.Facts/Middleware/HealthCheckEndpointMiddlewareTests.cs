using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class HealthCheckEndpointMiddlewareTests
    {
        private readonly MetricsTestFixture _fixture;

        public HealthCheckEndpointMiddlewareTests()
        {
            _fixture = new MetricsTestFixture();
        }

        [Fact]
        public async Task can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
           var result = await _fixture.Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}