using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Internal;
using App.Metrics.Json;
using App.Metrics.MetricData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
                DefaultGroupName = "testing",
                DisableMetrics = true,
                DisableHealthChecks = false,
                JsonSchemeVersion = JsonSchemeVersion.Version1
            });

            var result = await _fixture.Client.GetAsync("/metrics");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task can_change_metrics_endpoint()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultGroupName = "testing",
                DisableMetrics = false,
                DisableHealthChecks = false,
                JsonSchemeVersion = JsonSchemeVersion.Version1
            }, new AspNetMetricsOptions { MetricsEndpoint = new PathString("/metrics-json") });

            var result = await _fixture.Client.GetAsync("/metrics");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result = await _fixture.Client.GetAsync("/metrics-json");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task can_disable_metrics_endpoint_when_metrics_enabled()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultSamplingType = SamplingType.ExponentiallyDecaying,
                DefaultGroupName = "testing",
                DisableMetrics = false,
                DisableHealthChecks = false,
                JsonSchemeVersion = JsonSchemeVersion.Version1
            }, new AspNetMetricsOptions { MetricsEnabled = false });

            var result = await _fixture.Client.GetAsync("/metrics");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task can_filter_metrics()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultSamplingType = SamplingType.ExponentiallyDecaying,
                DefaultGroupName = "testing",
                DisableMetrics = false,
                DisableHealthChecks = true,
                JsonSchemeVersion = JsonSchemeVersion.Version1,
                MetricsFilter = new DefaultMetricsFilter().WhereType(MetricType.Counter)
            });

            var result = await _fixture.Client.GetAsync("/metrics");

            //TODO: AH - Deserialize to JsonMetricsContext and convert to MetricsData to confirm results

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}