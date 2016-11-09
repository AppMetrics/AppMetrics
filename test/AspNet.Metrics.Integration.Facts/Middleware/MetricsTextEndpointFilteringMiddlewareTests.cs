using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Facts.Integration.Startup;
using Xunit;
using FluentAssertions;

namespace AspNet.Metrics.Facts.Integration.Middleware
{
    public class MetricsTextEndpointFilteringMiddlewareTests : IClassFixture<MetricsHostTestFixture<FitleredMetricsEndpointStartup>>
    {
        public MetricsTextEndpointFilteringMiddlewareTests(MetricsHostTestFixture<FitleredMetricsEndpointStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact]
        public async Task can_filter_metrics()
        {
            var response = await Client.GetAsync("/metrics");

            var result = await response.Content.ReadAsStringAsync();

            //TODO: AH - deserialize reponse and confirm

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}