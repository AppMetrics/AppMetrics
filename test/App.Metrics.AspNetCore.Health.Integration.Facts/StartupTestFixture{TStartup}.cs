// <copyright file="StartupTestFixture{TStartup}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace App.Metrics.AspNetCore.Health.Integration.Facts
{
    public class StartupTestFixture<TStartup> : IDisposable
        where TStartup : class
    {
        private readonly TestServer _server;

        public StartupTestFixture()
        {
            var builder = new WebHostBuilder().UseStartup<TStartup>();
            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}