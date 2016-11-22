using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.OAuth2
{
    public class PerRequestTimerMiddlewareCountsTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public PerRequestTimerMiddlewareCountsTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_count_requests_per_endpoint_and_also_get_a_total_count()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/error");

            var metrics = await Context.Advanced.Data.ReadContextAsync(AspNetMetricsRegistry.Contexts.HttpRequests.ContextName);

            metrics.TimerValueFor("GET api/test").Histogram.Count.Should().Be(1);
            metrics.TimerValueFor("GET api/test/error").Histogram.Count.Should().Be(1);
            metrics.TimerValueFor("Http Requests").Histogram.Count.Should().Be(2);
        }
    }
}