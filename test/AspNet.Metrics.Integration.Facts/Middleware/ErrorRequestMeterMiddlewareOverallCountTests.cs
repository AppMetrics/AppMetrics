using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Integration.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Integration.Facts.Middleware
{
    public class ErrorRequestMeterMiddlewareOverallCountTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorRequestMeterMiddlewareOverallCountTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

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

            var metrics = await Context.Advanced.Data.ReadContextAsync("Application.WebRequests");

            metrics.MeterValueFor("Total Bad Requests").Count.Should().Be(3);
            metrics.MeterValueFor("Total Internal Server Error Requests").Count.Should().Be(2);
            metrics.MeterValueFor("Total UnAuthorized Requests").Count.Should().Be(1);
            metrics.MeterValueFor("Total Error Requests").Count.Should().Be(6);
            metrics.TimerValueFor("Web Requests").Histogram.Count.Should().Be(7);
        }
    }
}