using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class ErrorRequestMeterMiddlewareOverallCountTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorRequestMeterMiddlewareOverallCountTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact]
        public async Task can_count_overall_400_401_and_500_error_requests()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/unauth");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/error");
            await Client.GetAsync("/api/test/error");


            (await Context.MeterValueAsync("Application.WebRequests", "Total Bad Requests")).Count.Should().Be(3);
            (await Context.MeterValueAsync("Application.WebRequests", "Total Internal Server Error Requests")).Count.Should().Be(2);
            (await Context.MeterValueAsync("Application.WebRequests", "Total UnAuthorized Requests")).Count.Should().Be(1);
            (await Context.MeterValueAsync("Application.WebRequests", "Total Error Requests")).Count.Should().Be(6);
            (await Context.TimerValueAsync("Application.WebRequests", "Web Requests")).Histogram.Count.Should().Be(7);
        }
    }
}