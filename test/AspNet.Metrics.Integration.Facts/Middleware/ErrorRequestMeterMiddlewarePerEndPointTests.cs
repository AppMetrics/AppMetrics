using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Facts.Integration.Startup;
using Xunit;
using FluentAssertions;

namespace AspNet.Metrics.Facts.Integration.Middleware
{
    public class ErrorRequestMeterMiddlewarePerEndPointTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorRequestMeterMiddlewarePerEndPointTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_count_errors_per_endpoint_and_also_get_a_total_error_count()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/unauth");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/error");
            await Client.GetAsync("/api/test/error");

            var metrics = await Context.Advanced.DataManager.GetContextAsync("Application.WebRequests");

            metrics.MeterValueFor("GET api/test/bad Total Error Requests").Count.Should().Be(1);
            metrics.MeterValueFor("GET api/test/error Internal Server Error Requests").Count.Should().Be(2);
            metrics.MeterValueFor("GET api/test/unauth Total Error Requests").Count.Should().Be(1);
            metrics.MeterValueFor("Total Error Requests").Count.Should().Be(4);
            metrics.MeterValueFor("GET api/test/bad Total Error Requests").Count.Should().Be(1);
            metrics.MeterValueFor("Total Error Requests").Count.Should().Be(4);
            metrics.TimerValueFor("Web Requests").Histogram.Count.Should().Be(5);
        }
    }
}