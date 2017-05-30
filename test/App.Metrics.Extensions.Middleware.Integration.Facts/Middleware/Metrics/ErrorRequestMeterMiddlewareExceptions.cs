using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Metrics
{
    public class ErrorRequestMeterMiddlewareExceptions : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorRequestMeterMiddlewareExceptions(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        private HttpClient Client { get; }

        private IMetrics Context { get; }

        [Fact]
        public async Task can_count_requests_throwing_exceptions_as_errors()
        {
            try
            {
                await Client.GetAsync("/api/test/exception");
            }
            catch
            {
                // Do Nothing
            }

            var meterValueError = Context.Snapshot.GetMeterValue(
                HttpRequestMetricsRegistry.ContextName,
                "Error Rate Per Endpoint And Status Code|route:GET api/test/exception,http_status_code:500");

            meterValueError.Count.Should().Be(1L);
        }
    }
}