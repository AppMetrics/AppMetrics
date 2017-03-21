// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Gauge;
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

        private HttpClient Client { get; }

        private IMetrics Context { get; }

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

            double GetGaugeValue(string metricName) => Context.Snapshot.GetGaugeValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            GetGaugeValue("One Minute Error Percentage Rate Per Endpoint|route:GET api/test/error-random/{passorfail}").Should().BeApproximately(35, 5);
        }
    }
}