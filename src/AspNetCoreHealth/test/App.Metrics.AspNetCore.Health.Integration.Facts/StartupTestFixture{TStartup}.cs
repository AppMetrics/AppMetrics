// <copyright file="StartupTestFixture{TStartup}.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using App.Metrics.Health;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Health.Integration.Facts
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class StartupTestFixture<TStartup> : IDisposable
        // ReSharper restore ClassNeverInstantiated.Global
        where TStartup : class
    {
        private readonly TestServer _server;

        public StartupTestFixture()
        {
            var builder = new WebHostBuilder().UseStartup<TStartup>();

            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Health = _server.Host.Services.GetRequiredService<IHealth>();
        }

        public HttpClient Client { get; }

        public IHealth Health { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}