using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Data;
using AspNet.Metrics.Facts.Startup;
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

            var metrics = await Context.Advanced.DataManager.GetByGroupAsync("Application.WebRequests");

            metrics.MeterValue("Total Bad Requests").Count.Should().Be(3);
            metrics.MeterValue("Total Internal Server Error Requests").Count.Should().Be(2);
            metrics.MeterValue("Total UnAuthorized Requests").Count.Should().Be(1);
            metrics.MeterValue("Total Error Requests").Count.Should().Be(6);
            metrics.TimerValue("Web Requests").Histogram.Count.Should().Be(7);
        }
    }
}