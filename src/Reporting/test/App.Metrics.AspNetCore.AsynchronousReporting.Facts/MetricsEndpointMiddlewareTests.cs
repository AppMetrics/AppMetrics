using System.Net;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.AsynchronousReporting.Facts.Startup;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace App.Metrics.AspNetCore.AsynchronousReporting.Facts
{
    public class MetricsEndpointMiddlewareTests : IClassFixture<CustomWebApplicationFactory<PlainTextTestStartup>>,
        IClassFixture<CustomWebApplicationFactory<JsonTestStartup>>,
        IClassFixture<CustomWebApplicationFactory<PrometheusTestStartup>>
    {
        private readonly WebApplicationFactory<PlainTextTestStartup> _plainText;
        private readonly WebApplicationFactory<JsonTestStartup> _json;
        private readonly WebApplicationFactory<PrometheusTestStartup> _prometheus;

        public MetricsEndpointMiddlewareTests(
            CustomWebApplicationFactory<PlainTextTestStartup> plainText,
            CustomWebApplicationFactory<JsonTestStartup> json,
            CustomWebApplicationFactory<PrometheusTestStartup> prometheus)
        {
            _plainText = plainText;
            _json = json;
            _prometheus = prometheus;
        }

        [Fact]
        public async Task PlainText_metrics_endpoint_no_exceptions()
        {
            // Arrange
            var client = _plainText.CreateClient();

            // Act
            var result = await client.GetAsync("/metrics");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Json_metrics_endpoint_no_exceptions()
        {
            // Arrange
            var client = _json.CreateClient();

            // Act
            var result = await client.GetAsync("/metrics");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task Prometheus_metrics_endpoint_no_exceptions()
        {
            // Arrange
            var client = _prometheus.CreateClient();

            var response = await client.GetAsync("/test");
            
            // Act
            var result = await client.GetAsync("/metrics");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}