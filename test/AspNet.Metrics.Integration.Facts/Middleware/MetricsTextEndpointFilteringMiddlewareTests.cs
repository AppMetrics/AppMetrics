using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Integration.Facts.Middleware
{
    public class MetricsTextEndpointFilteringMiddlewareTests : IClassFixture<MetricsHostTestFixture<FitleredMetricsEndpointStartup>>
    {
        public MetricsTextEndpointFilteringMiddlewareTests(MetricsHostTestFixture<FitleredMetricsEndpointStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_filter_metrics()
        {
            var response = await Client.GetAsync("/metrics-text");

            var result = await response.Content.ReadAsStringAsync();

            result.Should().Contain("Counter");
            result.Should().NotContain("Gauge");
            result.Should().NotContain("Meter");
            result.Should().NotContain("Timers");
            result.Should().NotContain("Histogram");


            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}