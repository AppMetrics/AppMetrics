using App.Metrics;
using FluentAssertions;
using System.Threading.Tasks;
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

            var timer = _fixture.TestContext.TimerValue("Application.WebRequests", "Web Requests");
            timer.Histogram.Min.Should().Be(30);
            timer.Histogram.Max.Should().Be(300);
            timer.TotalTime.Should().Be(630);
        }
    }
}