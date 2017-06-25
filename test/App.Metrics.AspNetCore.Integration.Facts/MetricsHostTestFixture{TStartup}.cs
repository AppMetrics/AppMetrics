// <copyright file="MetricsHostTestFixture{TStartup}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using App.Metrics.Formatters.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Integration.Facts
{
    public class MetricsHostTestFixture<TStartup> : IDisposable
        where TStartup : class
    {
        private readonly TestServer _server;

        public MetricsHostTestFixture()
        {
            var builder = new WebHostBuilder().UseStartup<TStartup>();
            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Context = _server.Host.Services.GetRequiredService<IMetrics>();
            JsonMetricsSerializer = new MetricDataSerializer();
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        public MetricDataSerializer JsonMetricsSerializer { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}