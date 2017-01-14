using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Ping
{
    public class PingEndpointDisabledMiddlewareTests : IClassFixture<MetricsHostTestFixture<PingDisabledTestStartup>>
    {
        public PingEndpointDisabledMiddlewareTests(MetricsHostTestFixture<PingDisabledTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task when_enabled_returns_pong()
        {
            var result = await Client.GetAsync("/ping");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}