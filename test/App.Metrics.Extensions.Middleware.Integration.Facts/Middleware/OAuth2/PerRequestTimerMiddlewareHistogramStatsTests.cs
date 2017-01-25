using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.OAuth2
{
    public class PerRequestTimerMiddlewareHistogramStatsTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public PerRequestTimerMiddlewareHistogramStatsTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task can_record_times_per_request()
        {
            await Client.GetAsync("/api/test/300ms");
            await Client.GetAsync("/api/test/30ms");

            var metrics = Context.Snapshot.GetForContext(AspNetMetricsRegistry.Contexts.HttpRequests.ContextName);

            var timer1 = metrics.TimerValueFor("GET api/test/30ms");
            timer1.Histogram.Min.Should().Be(30);
            timer1.Histogram.Max.Should().Be(30);
            timer1.Histogram.Mean.Should().Be(30);
            timer1.Histogram.Percentile95.Should().Be(30);
            timer1.Histogram.Percentile98.Should().Be(30);
            timer1.Histogram.Percentile99.Should().Be(30);
            timer1.Histogram.Percentile999.Should().Be(30);
            timer1.TotalTime.Should().Be(30);

            var timer2 = metrics.TimerValueFor("GET api/test/300ms");
            timer2.Histogram.Min.Should().Be(300);
            timer2.Histogram.Max.Should().Be(300);
            timer2.Histogram.Mean.Should().Be(300);
            timer2.Histogram.Percentile75.Should().Be(300);
            timer2.Histogram.Percentile95.Should().Be(300);
            timer2.Histogram.Percentile98.Should().Be(300);
            timer2.Histogram.Percentile99.Should().Be(300);
            timer2.Histogram.Percentile999.Should().Be(300);
            timer2.TotalTime.Should().Be(300);
        }
    }
}