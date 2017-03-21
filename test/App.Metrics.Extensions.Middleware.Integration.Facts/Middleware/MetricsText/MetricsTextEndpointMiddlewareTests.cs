using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.MetricsText
{
    public class MetricsTextEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public MetricsTextEndpointMiddlewareTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task returns_correct_response_headers()
        {
            var result = await Client.GetAsync("/metrics-text");

            result.Headers.CacheControl.NoCache.Should().Be(true);
            result.Headers.CacheControl.NoStore.Should().Be(true);
            result.Headers.CacheControl.MustRevalidate.Should().Be(true);
            result.Headers.Pragma.ToString().Should().Be("no-cache");
        }

        [Fact]
        public async Task uses_correct_mimetype()
        {
            var result = await Client.GetAsync("/metrics-text");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "text/plain");
        }
    }
}