// <copyright file="Program.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ReportingSandbox.JustForTesting;
using Serilog;
using Serilog.Events;

namespace ReportingSandbox
{
    public static class Program
    {
        public static IMetricsRoot Metrics { get; private set; }

        public static Task Main()
        {
            Init();

            SampleMetricsRunner.ScheduleSomeSampleMetrics(Metrics);

            // Using AspNet Core to host a HTTP endpoint which receives metrics as JSON via the HTTP reporter.
            var host = new HostBuilder()
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("http://localhost:50001");
                    })
                .Build();

            host.Run();

            return Task.CompletedTask;
        }

        private static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Metrics = AppMetrics.CreateDefaultBuilder()
                .Report.ToConsole(TimeSpan.FromSeconds(3))
                .Report.ToTextFile(@"C:\metrics\sample.txt", TimeSpan.FromSeconds(5))
                .Report.OverHttp("http://localhost:50001/metrics-receive", TimeSpan.FromSeconds(50))
                // .Report.OverHttp("http://localhost:50002/api/metrics", TimeSpan.FromSeconds(10))
                // .Report.OverTcp(new MetricsInfluxDbLineProtocolOutputFormatter(), "localhost", 8094)
                // .Report.OverUdp(new MetricsInfluxDbLineProtocolOutputFormatter(), "localhost", 8094)
                // .Report.OverUds(new MetricsInfluxDbLineProtocolOutputFormatter(), "/tmp/telegraf.sock")
                .Build();
        }
    }
}