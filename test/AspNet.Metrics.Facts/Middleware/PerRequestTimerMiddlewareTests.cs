using App.Metrics;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class PerRequestTimerMiddlewareTests
    {
        private readonly MetricsTestFixture _fixture;

        public PerRequestTimerMiddlewareTests()
        {
            _fixture = new MetricsTestFixture();
        }

        [Fact]
        public async Task can_count_requests_per_endpoint_and_also_get_a_total_count()
        {
            await _fixture.Client.GetAsync("/api/test");
            await _fixture.Client.GetAsync("/api/test/error");


            var timer1 = await _fixture.TestContext.TimerValueAsync("Application.WebRequests", "GET api/test");
            timer1.Histogram.Count.Should().Be(1);

            var timer2 = await _fixture.TestContext.TimerValueAsync("Application.WebRequests", "GET api/test/error");
            timer2.Histogram.Count.Should().Be(1);


            var timer3 = await _fixture.TestContext.TimerValueAsync("Application.WebRequests", "Web Requests");
            timer3.Histogram.Count.Should().Be(2);
        }

        [Fact]
        public async Task can_record_times_per_request()
        {
            await _fixture.Client.GetAsync("/api/test/300ms");
            await _fixture.Client.GetAsync("/api/test/30ms");


            var timer1 = await _fixture.TestContext.TimerValueAsync("Application.WebRequests", "GET api/test/30ms");
            timer1.Histogram.Min.Should().Be(30);
            timer1.Histogram.Max.Should().Be(30);
            timer1.Histogram.Mean.Should().Be(30);
            timer1.Histogram.Percentile95.Should().Be(30);
            timer1.Histogram.Percentile98.Should().Be(30);
            timer1.Histogram.Percentile99.Should().Be(30);
            timer1.Histogram.Percentile999.Should().Be(30);
            timer1.TotalTime.Should().Be(30);

            var timer2 = await _fixture.TestContext.TimerValueAsync("Application.WebRequests", "GET api/test/300ms");
            timer2.Histogram.Min.Should().Be(300);
            timer2.Histogram.Max.Should().Be(300);
            timer2.Histogram.Mean.Should().Be(300);
            timer2.Histogram.Percentile75.Should().Be(300);
            timer2.Histogram.Percentile95.Should().Be(300);
            timer2.Histogram.Percentile98.Should().Be(300);
            timer2.Histogram.Percentile99.Should().Be(300);
            timer2.Histogram.Percentile999.Should().Be(300);
            timer2.TotalTime.Should().Be(300);

            var timer3 = await _fixture.TestContext.TimerValueAsync("Application.WebRequests", "Web Requests");
            timer3.Histogram.Min.Should().Be(30);
            timer3.Histogram.Max.Should().Be(300);
            timer3.TotalTime.Should().Be(330);
        }
    }
}