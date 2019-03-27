// <copyright file="PerRequestTimerMiddlewareHistogramStatsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using App.Metrics.AspNetCore.Internal;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.OAuth2
{
    public class PerRequestTimerMiddlewareHistogramStatsTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public PerRequestTimerMiddlewareHistogramStatsTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        private HttpClient Client { get; }

        private IMetrics Context { get; }

        [Fact]
        public async Task Can_record_times_per_request()
        {
            await Client.GetAsync("api/test/300ms");
            await Client.GetAsync("api/test/30ms");

            var timer1 = Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                "Transactions Per Endpoint|route:GET api/test/30ms");

            timer1.Histogram.Min.Should().Be(30);
            timer1.Histogram.Max.Should().Be(30);
            timer1.Histogram.Mean.Should().Be(30);
            timer1.Histogram.Percentile95.Should().Be(30);
            timer1.Histogram.Percentile98.Should().Be(30);
            timer1.Histogram.Percentile99.Should().Be(30);
            timer1.Histogram.Percentile999.Should().Be(30);
            timer1.Histogram.Sum.Should().Be(30);

            var timer2 = Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                "Transactions Per Endpoint|route:GET api/test/300ms");

            timer2.Histogram.Min.Should().Be(300);
            timer2.Histogram.Max.Should().Be(300);
            timer2.Histogram.Mean.Should().Be(300);
            timer2.Histogram.Percentile75.Should().Be(300);
            timer2.Histogram.Percentile95.Should().Be(300);
            timer2.Histogram.Percentile98.Should().Be(300);
            timer2.Histogram.Percentile99.Should().Be(300);
            timer2.Histogram.Percentile999.Should().Be(300);
            timer2.Histogram.Sum.Should().Be(300);
        }
    }
}