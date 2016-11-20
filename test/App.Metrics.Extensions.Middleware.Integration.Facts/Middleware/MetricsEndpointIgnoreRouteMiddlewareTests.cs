using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware
{
    public class MetricsEndpointIgnoreRouteMiddlewareTests : IClassFixture<MetricsHostTestFixture<IgnoredRouteTestStartup>>
    {
        public MetricsEndpointIgnoreRouteMiddlewareTests(MetricsHostTestFixture<IgnoredRouteTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_ignore_specified_routes()
        {
            var ignoreResult = await Client.GetAsync("/api/test/ignore");
            ignoreResult.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await Client.GetAsync("/metrics");
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var stringResult = await ignoreResult.Content.ReadAsStringAsync();
            stringResult.Should().NotContain("GET api/test/ignore");
        }
    }
}