using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Json;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class MetricsEndpointMiddlewareTests
    {
        private MetricsTestFixture _fixture;

        public MetricsEndpointMiddlewareTests()
        {
            _fixture = new MetricsTestFixture();
        }

        [Fact]
        public async Task uses_correct_mimetype_for_json_version()
        {
            var result = await _fixture.Client.GetAsync("/metrics");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "application/vnd.app.metrics.v1.metrics+json");
        }

        [Fact]
        public async Task can_disable_metrics()
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