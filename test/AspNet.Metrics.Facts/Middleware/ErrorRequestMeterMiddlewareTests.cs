using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class ErrorRequestMeterMiddlewareTests
    {
        private readonly MetricsTestFixture _fixture;

        public ErrorRequestMeterMiddlewareTests()
        {
            _fixture = new MetricsTestFixture();
        }

        [Fact]
        public async Task can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
            await _fixture.Client.GetAsync("/api/test");
            await _fixture.Client.GetAsync("/api/test/unauth");
            await _fixture.Client.GetAsync("/api/test/bad");
            await _fixture.Client.GetAsync("/api/test/error");
            await _fixture.Client.GetAsync("/api/test/error");


            _fixture.TestContext.MeterValue("Application.WebRequests", "GET api/test/bad Total Error Requests").Count.Should().Be(1);
            _fixture.TestContext.MeterValue("Application.WebRequests", "GET api/test/error Total Error Requests").Count.Should().Be(2);
            _fixture.TestContext.MeterValue("Application.WebRequests", "GET api/test/unauth Total Error Requests").Count.Should().Be(1);
            _fixture.TestContext.MeterValue("Application.WebRequests", "Total Error Requests").Count.Should().Be(4);
            _fixture.TestContext.TimerValue("Application.WebRequests", "Web Requests").Histogram.Count.Should().Be(5);
        }

        [Fact]
        public async Task can_count_overall_400_401_and_500_error_requests()
        {
            await _fixture.Client.GetAsync("/api/test");
            await _fixture.Client.GetAsync("/api/test/unauth");
            await _fixture.Client.GetAsync("/api/test/bad");
            await _fixture.Client.GetAsync("/api/test/bad");
            await _fixture.Client.GetAsync("/api/test/bad");
            await _fixture.Client.GetAsync("/api/test/error");
            await _fixture.Client.GetAsync("/api/test/error");


            _fixture.TestContext.MeterValue("Application.WebRequests", "Total Bad Requests").Count.Should().Be(3);
            _fixture.TestContext.MeterValue("Application.WebRequests", "Total Internal Server Error Requests").Count.Should().Be(2);
            _fixture.TestContext.MeterValue("Application.WebRequests", "Total Unauthorized Requests").Count.Should().Be(1);
            _fixture.TestContext.MeterValue("Application.WebRequests", "Total Error Requests").Count.Should().Be(6);
            _fixture.TestContext.TimerValue("Application.WebRequests", "Web Requests").Histogram.Count.Should().Be(7);
        }

        [Fact]
        public async Task can_count_total_400_401_and_500_error_requests_per_endpoint()
        {
            await _fixture.Client.GetAsync("/api/test");
            await _fixture.Client.GetAsync("/api/test/unauth");
            await _fixture.Client.GetAsync("/api/test/bad");
            await _fixture.Client.GetAsync("/api/test/bad");
            await _fixture.Client.GetAsync("/api/test/bad");
            await _fixture.Client.GetAsync("/api/test/error");
            await _fixture.Client.GetAsync("/api/test/error");


            _fixture.TestContext.MeterValue("Application.WebRequests", "GET api/test/bad Bad Requests").Count.Should().Be(3);
            _fixture.TestContext.MeterValue("Application.WebRequests", "GET api/test/error Internal Server Error Requests").Count.Should().Be(2);
            _fixture.TestContext.MeterValue("Application.WebRequests", "GET api/test/unauth Unauthorized Requests").Count.Should().Be(1);
            _fixture.TestContext.MeterValue("Application.WebRequests", "Total Error Requests").Count.Should().Be(6);
            _fixture.TestContext.TimerValue("Application.WebRequests", "Web Requests").Histogram.Count.Should().Be(7);
        }
    }
}