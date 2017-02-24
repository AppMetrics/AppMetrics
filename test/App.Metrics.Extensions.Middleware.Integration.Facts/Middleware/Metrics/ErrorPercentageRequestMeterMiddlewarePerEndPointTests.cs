using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Gauge;
using App.Metrics.Meter;
using App.Metrics.Meter.Extensions;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Metrics
{
    public class ErrorPercentageRequestMeterMiddlewarePerEndPointTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorPercentageRequestMeterMiddlewarePerEndPointTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        [Fact]
        public async Task calculates_error_percentages_per_endpoint()
        {
            for (var i = 0; i < 500; i++)
            {
                var passorfail = "pass";
                if (i % 3 == 0)
                {
                    passorfail = "fail";
                }
                await Client.GetAsync($"/api/test/error-random/{passorfail}");
            }

            Func<string, double> getGaugeValue = metricName => Context.Snapshot.GetGaugeValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            getGaugeValue("Endpoint Percentage Error Requests|route:GET api/test/error-random/{passorfail}").Should().BeApproximately(35, 5);
        }
    }
}
