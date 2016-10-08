using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class MetricsEndpointMiddlewareTests
    {
        private readonly MetricsTestFixture _fixture;

        public MetricsEndpointMiddlewareTests()
        {
            _fixture = new MetricsTestFixture();
        }

        [Fact]
        public async Task uses_correct_mimetype_for_json_version()
        {
            var result = await _fixture.Client.GetAsync("/metrics");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "application/vnd.app.metrics.v1.metrics+json");
        }
    }
}