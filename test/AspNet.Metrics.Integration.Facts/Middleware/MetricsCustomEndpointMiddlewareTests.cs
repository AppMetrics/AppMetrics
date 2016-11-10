using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Integration.Facts.Middleware
{
    public class MetricsCustomEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<CustomMetricsEndpointTestStartup>>
    {
        public MetricsCustomEndpointMiddlewareTests(MetricsHostTestFixture<CustomMetricsEndpointTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_change_metrics_endpoint()
        {
            var result = await Client.GetAsync("/metrics");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result = await Client.GetAsync("/metrics-json");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}