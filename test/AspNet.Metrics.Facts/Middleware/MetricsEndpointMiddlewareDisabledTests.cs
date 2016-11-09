using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class MetricsEndpointMiddlewareDisabledTests : IClassFixture<MetricsHostTestFixture<DisabledMetricsTestStartup>>
    {
        public MetricsEndpointMiddlewareDisabledTests(MetricsHostTestFixture<DisabledMetricsTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact]
        public async Task can_disable_metrics()
        {
           
            var result = await Client.GetAsync("/metrics");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}