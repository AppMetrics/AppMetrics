// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using App.Metrics.AspNetCore;
using MetricsReportingSandboxMvc.JustForTesting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsReportingSandboxMvc
{
    public static class Program
    {
        public static IHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            return Host.CreateDefaultBuilder(args)
                          .ConfigureMetricsWithDefaults(
                               builder =>
                               {
                                   // builder.MetricFields.Configure(fields => { fields.Apdex.Exclude(); });
                                   builder.Report.Using<SimpleConsoleMetricsReporter>(TimeSpan.FromSeconds(2));
                               })
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

        // ReSharper disable UnusedMember.Local - .UseMetrics(ObserveReporterErrors())
        private static Action<MetricsWebHostOptions> ObserveReporterErrors()
            // ReSharper restore UnusedMember.Local
        {
            return options => options.UnobservedTaskExceptionHandler = (sender, args2) => { Trace.WriteLine(args2.Exception); };
        }
    }
}