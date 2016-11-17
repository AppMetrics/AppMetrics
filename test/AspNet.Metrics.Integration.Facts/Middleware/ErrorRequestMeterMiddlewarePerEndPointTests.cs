using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Integration.Facts.Startup;
using AspNet.Metrics.Internal;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Integration.Facts.Middleware
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

            var metrics = await Context.Advanced.Data.ReadContextAsync(AspNetMetricsRegistry.Contexts.HttpRequests.ContextName);

            metrics.MeterValueFor("GET api/test/bad Http Error Requests").Count.Should().Be(1);
            metrics.MeterValueFor("GET api/test/error Http Error Requests").Count.Should().Be(2);
            metrics.MeterValueFor("GET api/test/unauth Http Error Requests").Count.Should().Be(1);
            metrics.MeterValueFor("Http Error Requests").Count.Should().Be(4);
            metrics.MeterValueFor("Http Error Requests").Items.Length.Should().Be(3, "there are three endpoints which had an unsuccessful status code");
            metrics.MeterValueFor("GET api/test/bad Http Error Requests").Count.Should().Be(1);
            metrics.MeterValueFor("Http Error Requests").Count.Should().Be(4);
            metrics.TimerValueFor("Http Requests").Histogram.Count.Should().Be(5);
        }
    }
}