// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsInfluxDBSandboxMvc
{
    public static class Program
    {
        public static IHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                                .Build();

            var influxOptions = new MetricsReportingInfluxDbOptions();
            configuration.GetSection(nameof(MetricsReportingInfluxDbOptions)).Bind(influxOptions);
            influxOptions.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();

            var metrics = AppMetrics.CreateDefaultBuilder()
                                    .Configuration.ReadFrom(configuration)
                                    .Report.ToInfluxDb(influxOptions)
                                    .Build();

            return Host.CreateDefaultBuilder(args)
                          .ConfigureMetrics(metrics)
                          .UseMetrics()
                          .UseSerilog()
                          .ConfigureWebHostDefaults(
                              webBuilder =>
                              {
                                  webBuilder.UseStartup<Startup>();
                              })
                          .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static void ConfigureLogging()
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