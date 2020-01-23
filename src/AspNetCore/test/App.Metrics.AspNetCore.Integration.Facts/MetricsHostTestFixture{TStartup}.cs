// <copyright file="MetricsHostTestFixture{TStartup}.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Integration.Facts
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsHostTestFixture<TStartup> : IDisposable
        // ReSharper restore ClassNeverInstantiated.Global
        where TStartup : class
    {
        private readonly TestServer _server;

        public MetricsHostTestFixture()
        {
            var builder = new WebHostBuilder().UseStartup<TStartup>();
            _server = new TestServer(builder);
            _server.AllowSynchronousIO = true;

            Client = _server.CreateClient();
            Context = _server.Host.Services.GetRequiredService<IMetrics>();
        }

        public HttpClient Client { get; }

        public IMetrics Context { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}