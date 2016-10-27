using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
                DefaultGroupName = "testing",
                DisableMetrics = false,
                JsonSchemeVersion = JsonSchemeVersion.Version1
            }, enableHealthChecks:false);

            var result = await _fixture.Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task can_disable_health_endpoint_when_health_enabled()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultGroupName = "testing",
                DisableMetrics = false,
                JsonSchemeVersion = JsonSchemeVersion.Version1
            }, new AspNetMetricsOptions { HealthEndpointEnabled = false });

            var result = await _fixture.Client.GetAsync("/health");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task can_change_health_endpoint()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultGroupName = "testing",
                DisableMetrics = false,
                JsonSchemeVersion = JsonSchemeVersion.Version1
            }, new AspNetMetricsOptions {HealthEndpoint = new PathString("/health-check")});

            var result = await _fixture.Client.GetAsync("/health");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result = await _fixture.Client.GetAsync("/health-check");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}