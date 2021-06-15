// <copyright file="Program.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Reporting.StatsD;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsStatsDSandboxMvc
{
    public class Program
    {
        public static IHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).
                                                           AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true).Build();

            var statsDOptions = new MetricsReportingStatsDOptions();
            configuration.GetSection(nameof(MetricsReportingStatsDOptions)).Bind(statsDOptions);

            // Samples with weight of less than 10% of average should be discarded when rescaling
            const double minimumSampleWeight = 0.001;

            var metrics = AppMetrics.CreateDefaultBuilder().Configuration.ReadFrom(configuration).SampleWith.ForwardDecaying(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                minimumSampleWeight: minimumSampleWeight).Report.ToStatsDUdp(statsDOptions).Build();

            return Host.CreateDefaultBuilder(args).ConfigureMetrics(metrics).UseMetrics().UseSerilog().ConfigureWebHostDefaults(
                webBuilder => { webBuilder.UseStartup<Startup>(); }).Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().MinimumLevel.Override("Microsoft", LogEventLevel.Verbose).WriteTo.
                                                   LiterateConsole().WriteTo.Seq("http://localhost:5341").CreateLogger();
        }
    }
}