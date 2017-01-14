using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Ping
{
    public class PingEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<PingTestStartup>>
    {
        public PingEndpointMiddlewareTests(MetricsHostTestFixture<PingTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task resposne_is_plain_text_content_type()
        {
            var result = await Client.GetAsync("/ping");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "text/plain");
        }

        [Fact]
        public async Task returns_correct_response_headers()
        {
            var result = await Client.GetAsync("/ping");

            result.Headers.CacheControl.NoCache.Should().Be(true);
            result.Headers.CacheControl.NoStore.Should().Be(true);
            result.Headers.CacheControl.MustRevalidate.Should().Be(true);
            result.Headers.Pragma.ToString().Should().Be("no-cache");
        }

        [Fact]
        public async Task when_enabled_returns_pong()
        {
            var result = await Client.GetAsync("/ping");
            var response = await result.Content.ReadAsStringAsync();

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().Be("pong");
        }
    }
}