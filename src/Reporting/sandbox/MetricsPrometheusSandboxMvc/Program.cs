// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsPrometheusSandboxMvc
{
    public static class Program
    {
        public static IMetricsRoot Metrics { get; set; }

        public static IHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            Metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .OutputMetrics.AsPrometheusProtobuf()
                .Build();

            return Host.CreateDefaultBuilder(args)
                          .ConfigureMetrics(Metrics)
                          .UseMetrics(
                              options =>
                              {
                                  options.EndpointOptions = endpointsOptions =>
                                  {
                                      endpointsOptions.MetricsTextEndpointOutputFormatter = Metrics.OutputMetricsFormatters.GetType<MetricsPrometheusTextOutputFormatter>();
                                      endpointsOptions.MetricsEndpointOutputFormatter = Metrics.OutputMetricsFormatters.GetType<MetricsPrometheusTextOutputFormatter>();
                                      // endpointsOptions.MetricsEndpointOutputFormatter = Metrics.OutputMetricsFormatters.GetType<MetricsPrometheusProtobufOutputFormatter>();
                                  };
                              })
                          .UseSerilog()
                          .ConfigureWebHostDefaults(
                              webBuilder =>
                              {
                                  webBuilder.UseStartup<Startup>();
                                  webBuilder.UseUrls("http://localhost:1111");

                              })
                          .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .CreateLogger();
        }
    }
}