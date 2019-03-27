// <copyright file="ErrorRequestMeterMiddlewareExceptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.Metrics
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
        public async Task Can_count_requests_throwing_exceptions_as_errors()
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