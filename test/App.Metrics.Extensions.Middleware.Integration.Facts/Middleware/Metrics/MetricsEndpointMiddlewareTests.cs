using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Metrics
{
    public class MetricsEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public MetricsEndpointMiddlewareTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task uses_correct_mimetype_for_json_version()
        {
            var result = await Client.GetAsync("/metrics");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "application/vnd.app.metrics.v1.metrics+json");
        }

        [Fact]
        public async Task returns_correct_response_headers()
        {
            var result = await Client.GetAsync("/health");

            result.Headers.CacheControl.NoCache.Should().Be(true);
            result.Headers.CacheControl.NoStore.Should().Be(true);
            result.Headers.CacheControl.MustRevalidate.Should().Be(true);
            result.Headers.Pragma.ToString().Should().Be("no-cache");
        }
    }
}