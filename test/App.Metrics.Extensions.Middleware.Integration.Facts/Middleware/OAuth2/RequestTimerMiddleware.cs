// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.OAuth2
{
    public class RequestTimerMiddlewareTests : IClassFixture<MetricsHostTestFixture<DefaultTestStartup>>
    {
        public RequestTimerMiddlewareTests(MetricsHostTestFixture<DefaultTestStartup> fixture)
        {
            Client = fixture.Client;
            Context = fixture.Context;
        }

        private HttpClient Client { get; }

        private IMetrics Context { get; }

        [Fact]
        public async Task record_request_times()
        {
            await Client.GetAsync("/api/test/300ms");
            await Client.GetAsync("/api/test/300ms");
            await Client.GetAsync("/api/test/30ms");

            var timerValue = Context.Snapshot.GetTimerValue(
                HttpRequestMetricsRegistry.ContextName,
                "Transactions");

            timerValue.Histogram.Min.Should().Be(30);
            timerValue.Histogram.Max.Should().Be(300);
            timerValue.Histogram.Sum.Should().Be(630);
        }
    }
}