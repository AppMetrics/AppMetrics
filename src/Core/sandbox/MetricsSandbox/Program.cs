// <copyright file="Program.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using static System.Console;

namespace MetricsSandbox
{
    public static class Program
    {
        private static readonly bool FilterMetricValueTypes = true;

        private static readonly Random Rnd = new Random();

        private static IMetricsRoot Metrics { get; set; }

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.LiterateConsole(LogEventLevel.Information)
                         .WriteTo.Seq("http://localhost:5341")
                         .CreateLogger();

            var host = new HostBuilder()
               .ConfigureAppConfiguration(
                    (hostContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddEnvironmentVariables();
                        config.AddJsonFile("appsettings.json", optional: true);
                        config.AddCommandLine(args);
                    })
                .ConfigureServices(
                           (hostContext, services) =>
                           {
                               services.AddSingleton<IHostedService, RecordAndReportHostedService>();
                               services.ConfigureMetrics(hostContext, FilterMetricValueTypes);
                           })
                .Build();

            var cancellationTokenSource = new CancellationTokenSource();

            Metrics = host.Services.GetRequiredService<IMetricsRoot>();

            await WriteEnvAsync(cancellationTokenSource);

            PressAnyKeyToContinue();

            await WriteMetricsAsync(cancellationTokenSource);

            PressAnyKeyToContinue();

            await host.RunAsync(token: cancellationTokenSource.Token);
        }

        private static void PressAnyKeyToContinue()
        {
            WriteLine();
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Blue;
            WriteLine("Press any key to continue...");
            ResetColor();
            ReadKey();
            Clear();
        }

        private static async Task WriteEnvAsync(CancellationTokenSource cancellationTokenSource)
        {
            WriteLine("Environment Information");
            WriteLine("-------------------------------------------");

            foreach (var formatter in Metrics.OutputEnvFormatters)
            {
                WriteLine($"Formatter: {formatter.GetType().FullName}");
                WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    await formatter.WriteAsync(stream, Metrics.EnvironmentInfo, cancellationTokenSource.Token);

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    WriteLine(result);
                }
            }
        }

        private static async Task WriteMetricsAsync(CancellationTokenSource cancellationTokenSource)
        {
            foreach (var unused in Enumerable.Range(0, 10))
            {
                Metrics.Measure.Apdex.Track(ApplicationsMetricsRegistry.ApdexOne, () => Metrics.Clock.Advance(TimeUnit.Milliseconds, Rnd.Next(5000)));
                Metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterOne);
                Metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterWithSetItems, "item1");
                Metrics.Measure.Gauge.SetValue(ApplicationsMetricsRegistry.GaugeOne, Rnd.Next(0, 100));
                Metrics.Measure.Histogram.Update(ApplicationsMetricsRegistry.HistogramOne, Rnd.Next(0, 100));
                Metrics.Measure.BucketHistogram.Update(ApplicationsMetricsRegistry.BucketHistogramOne, Rnd.Next(0, 100));
                Metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterOne, Rnd.Next(0, 100));
                Metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterWithSetItems, Rnd.Next(0, 100), "item1");
            }

            var metricsData = Metrics.Snapshot.Get();

            WriteLine("Metrics Formatters");
            WriteLine("-------------------------------------------");

            foreach (var formatter in Metrics.OutputMetricsFormatters)
            {
                WriteLine($"Formatter: {formatter.GetType().FullName}");
                WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    await formatter.WriteAsync(stream, metricsData, cancellationTokenSource.Token);

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    WriteLine(result);
                }
            }
        }
    }
}