// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Middleware.Integration.Facts.Startup;
using App.Metrics.Formatters.Json.Serialization;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Middleware.Metrics
{
    public class MetricsEndpointFilteringMiddlewareTests : IClassFixture<MetricsHostTestFixture<FitleredMetricsEndpointStartup>>
    {
        public MetricsEndpointFilteringMiddlewareTests(MetricsHostTestFixture<FitleredMetricsEndpointStartup> fixture)
        {
            Client = fixture.Client;
            JsonMetricsSerializer = fixture.JsonMetricsSerializer;
        }

        private HttpClient Client { get; }

        private MetricDataSerializer JsonMetricsSerializer { get; }

        [Fact]
        public async Task can_filter_metrics()
        {
            var response = await Client.GetAsync("/metrics");

            var result = await response.Content.ReadAsStringAsync();

            var metrics = JsonMetricsSerializer.Deserialize<MetricsDataValueSource>(result);

            metrics.Contexts.Any(c => c.Counters.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Gauges.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Meters.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Timers.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Histograms.Any()).Should().BeTrue();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}