using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Json;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class HealthCheckEndpointMiddlewareTests
    {
        private MetricsTestFixture _fixture;

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

        [Fact]
        public async Task can_disable_health_checks()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultSamplingType = SamplingType.Default,
                GlobalContextName = "testing",
                DisableMetrics = false,
                DisableHealthChecks = true,
                JsonSchemeVersion = JsonSchemeVersion.Version1
            });

            var result = await _fixture.Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}