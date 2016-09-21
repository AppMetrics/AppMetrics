using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class PerRequestTimerTests : IClassFixture<MetricsTestFixture>
    {
        private readonly MetricsTestFixture _fixture;

        public PerRequestTimerTests(MetricsTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test()
        {
            await _fixture.Client.GetAsync("/api/test");
            await _fixture.Client.GetAsync("/api/test/error");


            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test").Histogram.Count.Should().Be(1);
            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/error").Histogram.Count.Should().Be(1);
            _fixture.TestContext.TimerValue("Application.WebRequests", "Web Requests").Histogram.Count.Should().Be(2);
        }
    }
}