using System.Threading.Tasks;
using AspNet.Metrics.Infrastructure;
using FluentAssertions;
using Metrics;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class PerRequestTimerMiddlewareTests //: IClassFixture<MetricsTestFixture>
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


            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test").Histogram.Count.Should().Be(1);
            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/error").Histogram.Count.Should().Be(1);
            _fixture.TestContext.TimerValue("Application.WebRequests", "Web Requests").Histogram.Count.Should().Be(2);
        }

        [Fact]
        public async Task can_record_times_per_request()
        {
            await _fixture.Client.GetAsync("/api/test/300ms");
            await _fixture.Client.GetAsync("/api/test/30ms");


            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/30ms").Histogram.Min.Should().BeLessThan(100);
            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/30ms").Histogram.Max.Should().BeLessThan(100);
            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/30ms").Histogram.Mean.Should().BeLessThan(100);

            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/300ms").Histogram.Min.Should().BeGreaterOrEqualTo(300);
            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/300ms").Histogram.Max.Should().BeGreaterOrEqualTo(300);
            _fixture.TestContext.TimerValue("Application.WebRequests", "GET api/test/300ms").Histogram.Mean.Should().BeGreaterOrEqualTo(300);
        }
    }
}