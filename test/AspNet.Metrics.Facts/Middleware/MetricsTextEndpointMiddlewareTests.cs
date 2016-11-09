using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using AspNet.Metrics.Facts.Startup;
using FluentAssertions;
using Xunit;

namespace AspNet.Metrics.Facts.Middleware
{
    public class MetricsTextEndpointMiddlewareTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public MetricsTextEndpointMiddlewareTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetricsContext Context { get; }

        [Fact(Skip = "Requires too man test startup classes, how can options be passed to IClassTestFixture?")]
        public async Task can_disable_metrics_text_endpoint_when_metrics_disabled()
        {
            //_fixture = new MetricsTestFixture(new AppMetricsOptions
            //{
            //    DefaultGroupName = "testing",
            //    DisableMetrics = true,
            //    DefaultSamplingType = SamplingType.LongTerm
            //});

            //var result = await _fixture.Client.GetAsync("/metrics-text");

            //result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Requires too man test startup classes, how can options be passed to IClassTestFixture?")]
        public async Task can_disable_metrics_text_endpoint_when_metrics_enabled()
        {
            //_fixture = new MetricsTestFixture(new AppMetricsOptions
            //{
            //    DefaultGroupName = "testing",
            //    DisableMetrics = false,
            //    DefaultSamplingType = SamplingType.LongTerm
            //}, new AspNetMetricsOptions { MetricsTextEndpointEnabled = false});

            //var result = await _fixture.Client.GetAsync("/metrics-text");

            //result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(Skip = "Requires too man test startup classes, how can options be passed to IClassTestFixture?")]
        public async Task can_filter_metrics_text()
        {
            //_fixture = new MetricsTestFixture(new AppMetricsOptions
            //{
            //    DefaultGroupName = "testing",
            //    DisableMetrics = false,
            //    DefaultSamplingType = SamplingType.LongTerm,
            //    MetricsFilter = new DefaultMetricsFilter().WhereType(MetricType.Counter)
            //});

            //var result = await _fixture.Client.GetAsync("/metrics-text");

            ////TODO: AH - Deserialize to JsonMetricsContext and convert to MetricsData to confirm results

            //result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task uses_correct_mimetype()
        {
            var result = await Client.GetAsync("/metrics-text");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Content.Headers.ContentType.ToString().Should().Match<string>(s => s == "text/plain");
        }
    }
}