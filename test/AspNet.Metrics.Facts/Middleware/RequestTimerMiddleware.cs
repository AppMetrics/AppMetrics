using System;
using System.Net.Http;
using App.Metrics;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class RequestTimerMiddlewareTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public RequestTimerMiddlewareTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact]
        public async Task record_request_times()
        {
            await Client.GetAsync("/api/test/300ms");
            await Client.GetAsync("/api/test/300ms");
            await Client.GetAsync("/api/test/30ms");

            var timer = await Context.TimerValueAsync("Application.WebRequests", "Web Requests");
            timer.Histogram.Min.Should().Be(30);
            timer.Histogram.Max.Should().Be(300);
            timer.TotalTime.Should().Be(630);
        }
    }
}