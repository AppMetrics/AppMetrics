// <copyright file="Program.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsReceiveSanboxApi
{
    public static class Program
    {
        public static IMetricsRoot Metrics => null;

        public static Task Main()
        {
            Init();

            var host = new HostBuilder()
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("http://localhost:50002");
                    })
                .Build();

            host.Run();

            return Task.CompletedTask;
        }

        private static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                         .WriteTo.LiterateConsole()
                         .WriteTo.Seq("http://localhost:5341")
                         .CreateLogger();
        }
    }
}