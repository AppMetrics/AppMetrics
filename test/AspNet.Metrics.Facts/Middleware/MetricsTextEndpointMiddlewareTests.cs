using System.Net;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Data;
using App.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class MetricsTextEndpointMiddlewareTests
    {
        private MetricsTestFixture _fixture;

        public MetricsTextEndpointMiddlewareTests()
        {
            _fixture = new MetricsTestFixture();
        }

        [Fact]
        public async Task uses_correct_mimetype()
        {
            var result = await _fixture.Client.GetAsync("/metrics-text");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "text/plain");
        }

        [Fact]
        public async Task can_disable_metrics_text_endpoint_when_metrics_disabled()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultGroupName = "testing",
                DisableMetrics = true
            });

            var result = await _fixture.Client.GetAsync("/metrics-text");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task can_disable_metrics_text_endpoint_when_metrics_enabled()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultGroupName = "testing",
                DisableMetrics = false
            }, new AspNetMetricsOptions { MetricsTextEndpointEnabled = false});

            var result = await _fixture.Client.GetAsync("/metrics-text");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task can_filter_metrics_text()
        {
            _fixture = new MetricsTestFixture(new AppMetricsOptions
            {
                DefaultGroupName = "testing",
                DisableMetrics = false,
                MetricsFilter = new DefaultMetricsFilter().WhereType(MetricType.Counter)
            });

            var result = await _fixture.Client.GetAsync("/metrics-text");

            //TODO: AH - Deserialize to JsonMetricsContext and convert to MetricsData to confirm results

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}