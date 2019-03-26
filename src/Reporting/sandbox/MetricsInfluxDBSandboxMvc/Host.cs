// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Health;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace MetricsInfluxDBSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
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

            return WebHost.CreateDefaultBuilder(args)
                          .ConfigureMetrics(metrics)
                          .ConfigureHealthWithDefaults(
                              (context, builder) =>
                              {
                                  builder.OutputHealth.AsPlainText()
                                         .OutputHealth.AsJson()
                                         .HealthChecks.AddCheck("check 1", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                                         .HealthChecks.AddCheck("check 2", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded()))
                                         .HealthChecks.AddCheck("check 3", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy()))
                                         .Report.ToMetrics(metrics);
                              })
                          .UseHealth()
                          .UseMetrics()
                          .UseSerilog()
                          .UseStartup<Startup>()
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