using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Data;
using App.Metrics.Formatters.Json;
using AspNet.Metrics.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Integration.Facts.Middleware
{
    public class MetricsEndpointFilteringMiddlewareTests : IClassFixture<MetricsHostTestFixture<FitleredMetricsEndpointStartup>>
    {
        public MetricsEndpointFilteringMiddlewareTests(MetricsHostTestFixture<FitleredMetricsEndpointStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
            JsonMetricsSerializer = fixture.JsonMetricsSerializer;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        public MetricDataSerializer JsonMetricsSerializer { get; }

        [Fact]
        public async Task can_filter_metrics()
        {
            var response = await Client.GetAsync("/metrics");

            var result = await response.Content.ReadAsStringAsync();

            var metrics = JsonMetricsSerializer.Deserialize<MetricsDataValueSource>(result);

            metrics.Contexts.Any(c => c.Counters.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Gauges.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Meters.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Timers.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Histograms.Any()).Should().BeTrue();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}