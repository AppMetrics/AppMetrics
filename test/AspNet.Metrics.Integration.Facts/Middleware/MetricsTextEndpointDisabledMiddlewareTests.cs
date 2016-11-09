using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Facts.Integration.Startup;
using Xunit;
using FluentAssertions;

namespace AspNet.Metrics.Facts.Integration.Middleware
{
    public class MetricsTextEndpointDisabledMiddlewareTests : IClassFixture<MetricsHostTestFixture<DisabledTextEndpointTestStartup>>
    {
        public MetricsTextEndpointDisabledMiddlewareTests(MetricsHostTestFixture<DisabledTextEndpointTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact]
        public async Task can_disable_metrics_text_endpoint_when_metrics_enabled()
        {
            var result = await Client.GetAsync("/metrics-text");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}