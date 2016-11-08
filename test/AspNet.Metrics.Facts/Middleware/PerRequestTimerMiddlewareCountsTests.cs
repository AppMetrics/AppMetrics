using System;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using Xunit;
using FluentAssertions;

namespace AspNet.Metrics.Facts.Middleware
{
    public class PerRequestTimerMiddlewareCountsTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public PerRequestTimerMiddlewareCountsTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact]
        public async Task can_count_requests_per_endpoint_and_also_get_a_total_count()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/error");

            var timer1 = await Context.TimerValueAsync("Application.WebRequests", "GET api/test");
            timer1.Histogram.Count.Should().Be(1);

            var timer2 = await Context.TimerValueAsync("Application.WebRequests", "GET api/test/error");
            timer2.Histogram.Count.Should().Be(1);

            var timer3 = await Context.TimerValueAsync("Application.WebRequests", "Web Requests");
            timer3.Histogram.Count.Should().Be(2);
        }

             
    }
}