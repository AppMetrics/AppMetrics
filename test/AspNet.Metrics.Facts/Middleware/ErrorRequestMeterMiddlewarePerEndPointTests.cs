using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class ErrorRequestMeterMiddlewarePerEndPointTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorRequestMeterMiddlewarePerEndPointTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact]
        public async Task can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/unauth");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/error");
            await Client.GetAsync("/api/test/error");


            (await Context.MeterValueAsync("Application.WebRequests", "GET api/test/bad Total Error Requests")).Count.Should().Be(1);
            (await Context.MeterValueAsync("Application.WebRequests", "GET api/test/error Internal Server Error Requests")).Count.Should().Be(2);
            (await Context.MeterValueAsync("Application.WebRequests", "GET api/test/unauth Total Error Requests")).Count.Should().Be(1);
            (await Context.MeterValueAsync("Application.WebRequests", "Total Error Requests")).Count.Should().Be(4);
            (await Context.TimerValueAsync("Application.WebRequests", "Web Requests")).Histogram.Count.Should().Be(5);
        }
    }   
}