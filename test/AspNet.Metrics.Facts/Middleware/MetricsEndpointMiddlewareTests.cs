using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Data;
using App.Metrics.Internal;
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
                DefaultSamplingType = SamplingType.LongTerm
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
                DefaultSamplingType = SamplingType.LongTerm
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
                DefaultSamplingType = SamplingType.LongTerm,
                DefaultGroupName = "testing",
                DisableMetrics = false
            }, new AspNetMetricsOptions { MetricsEndpointEnabled = false });

            var result = await _fixture.Client.GetAsync("/metrics");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task can_filter_metrics()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultSamplingType = SamplingType.LongTerm,
                DefaultGroupName = "testing",
                DisableMetrics = false,
                MetricsFilter = new DefaultMetricsFilter().WhereType(MetricType.Counter)
            });

            var result = await _fixture.Client.GetAsync("/metrics");

            //TODO: AH - Deserialize to JsonMetricsContext and convert to MetricsData to confirm results

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}