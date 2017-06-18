// <copyright file="PerRequestTimerMiddlewareCountsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Core.Timer;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Middleware.Internal;
using App.Metrics.Timer;
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

        private HttpClient Client { get; }

        private IMetrics Context { get; }

        [Fact]
        public async Task Can_count_requests_per_endpoint_and_also_get_a_total_count()
        {
            await Client.GetAsync("/api/test");
            await Client.GetAsync("/api/test/error");

            TimerValue GetTimerValue(string metricName) => Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                metricName);

            GetTimerValue("Transactions Per Endpoint|route:GET api/test").Histogram.Count.Should().Be(1);
            GetTimerValue("Transactions Per Endpoint|route:GET api/test/error").Histogram.Count.Should().Be(1);
            GetTimerValue("Transactions").Histogram.Count.Should().Be(2);
        }
    }
}