using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class RequestTimerMiddleware
    {
        private readonly MetricsTestFixture _fixture;

        public RequestTimerMiddleware()
        {
            _fixture = new MetricsTestFixture();
        }

        [Fact]
        public async Task record_request_times()
        {
            await _fixture.Client.GetAsync("/api/test/300ms");
            await _fixture.Client.GetAsync("/api/test/300ms");
            await _fixture.Client.GetAsync("/api/test/30ms");

            var timer3 = _fixture.TestContext.TimerValue("Application.WebRequests", "Web Requests");
            timer3.Histogram.Min.Should().Be(30);
            timer3.Histogram.Max.Should().Be(300);
            timer3.TotalTime.Should().Be(630);
        }
    }
}