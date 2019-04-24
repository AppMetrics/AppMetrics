// <copyright file="ErrorRequestMeterMiddlewareOverallCountTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.Meter;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.Metrics
{
    public class ErrorRequestMeterMiddlewareOverallCountTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public ErrorRequestMeterMiddlewareOverallCountTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        private HttpClient Client { get; }

        private IMetrics Context { get; }

        [Fact]
        public async Task Can_count_overall_400_401_and_500_error_requests()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/unauth");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/bad");
            await Client.GetAsync("/api/test/error");
            await Client.GetAsync("/api/test/error");

            var meterValueBad = Context.Snapshot.GetMeterValue(
                HttpRequestMetricsRegistry.ContextName,
                "Error Rate Per Endpoint And Status Code|route:GET api/test/bad,http_status_code:400");

            var timerValueBad = Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                "Transactions Per Endpoint|route:GET api/test/bad");

            meterValueBad.Count.Should().Be(3L);
            timerValueBad.Histogram.Count.Should().Be(3L);

            var meterValueError = Context.Snapshot.GetMeterValue(
                HttpRequestMetricsRegistry.ContextName,
                "Error Rate Per Endpoint And Status Code|route:GET api/test/error,http_status_code:500");

            var timerValueError = Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                "Transactions Per Endpoint|route:GET api/test/error");

            meterValueError.Count.Should().Be(2L);
            timerValueError.Histogram.Count.Should().Be(2L);

            var meterValueUnauth = Context.Snapshot.GetMeterValue(
                HttpRequestMetricsRegistry.ContextName,
                "Error Rate Per Endpoint And Status Code|route:GET api/test/unauth,http_status_code:401");

            var timerValueUnauth = Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                "Transactions Per Endpoint|route:GET api/test/unauth");

            meterValueUnauth.Count.Should().Be(1L);
            timerValueUnauth.Histogram.Count.Should().Be(1L);

            var timerValueOk = Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                "Transactions Per Endpoint|route:GET api/test");

            timerValueOk.Histogram.Count.Should().Be(1L);
        }
    }
}