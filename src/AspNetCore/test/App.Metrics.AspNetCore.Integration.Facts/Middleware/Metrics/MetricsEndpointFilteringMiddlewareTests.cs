// <copyright file="MetricsEndpointFilteringMiddlewareTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Integration.Facts.Startup;
using App.Metrics.Formatters.Json;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace App.Metrics.AspNetCore.Integration.Facts.Middleware.Metrics
{
    public class MetricsEndpointFilteringMiddlewareTests : IClassFixture<MetricsHostTestFixture<FitleredMetricsEndpointStartup>>
    {
        public MetricsEndpointFilteringMiddlewareTests(MetricsHostTestFixture<FitleredMetricsEndpointStartup> fixture)
        {
            Client = fixture.Client;
        }

        private HttpClient Client { get; }

        [Fact]
        public async Task Can_filter_metrics()
        {
            var response = await Client.GetAsync("/metrics");

            var result = await response.Content.ReadAsStringAsync();

            var metrics = JsonConvert.DeserializeObject<MetricsDataValueSource>(result);

            metrics.Contexts.Any(c => c.Counters.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Gauges.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Meters.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Timers.Any()).Should().BeTrue();
            metrics.Contexts.All(c => !c.Histograms.Any()).Should().BeTrue();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}