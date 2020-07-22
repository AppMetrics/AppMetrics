// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Reporting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GrafanaCloudHostedMetricsSandbox
{
    public static class Host
    {
        private const string GrafanaCloudHostedMetricsApiKey = "add api key here";
        private const string GrafanaCloudHostedMetricsUri = "add hosted metrics uri here";
        private static readonly Random Rnd = new Random();

        private static IConfigurationRoot Configuration { get; set; }

        private static IMetricsRoot Metrics { get; set; }

        private static IRunMetricsReports Reporter { get; set; }

        public static async Task Main()
        {
            Init();

            var cancellationTokenSource = new CancellationTokenSource();

            await WriteMetricsAsync(cancellationTokenSource);

            PressAnyKeyToContinue();

            await RunUntilEscAsync(
                TimeSpan.FromSeconds(5),
                cancellationTokenSource,
                async () =>
                {
                    Console.Clear();
                    RecordMetrics();
                    await Task.WhenAll(Reporter.RunAllAsync(cancellationTokenSource.Token));
                });
        }

        private static void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        private static async Task WriteMetricsAsync(CancellationTokenSource cancellationTokenSource)
        {
            foreach (var unused in Enumerable.Range(0, 10))
            {
                RecordMetrics();
            }

            var metricsData = Metrics.Snapshot.Get();

            Console.WriteLine("Metrics Formatters");
            Console.WriteLine("-------------------------------------------");

            foreach (var formatter in Metrics.OutputMetricsFormatters)
            {
                Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                Console.WriteLine("-------------------------------------------");

                await using var stream = new MemoryStream();
                await formatter.WriteAsync(stream, metricsData, cancellationTokenSource.Token);
                var result = Encoding.UTF8.GetString(stream.ToArray());

                Console.WriteLine(result);
            }
        }

        private static void RecordMetrics()
        {
            Metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterOne);
            Metrics.Measure.Gauge.SetValue(ApplicationsMetricsRegistry.GaugeOne, Rnd.Next(0, 100));
            Metrics.Measure.Histogram.Update(ApplicationsMetricsRegistry.HistogramOne, Rnd.Next(0, 100));
            Metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterOne, Rnd.Next(0, 100));

            using (Metrics.Measure.Timer.Time(ApplicationsMetricsRegistry.TimerOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }

            using (Metrics.Measure.Apdex.Track(ApplicationsMetricsRegistry.ApdexOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }
        }

        private static void Init()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            var metricsConfigSection = Configuration.GetSection(nameof(MetricsOptions));

            Metrics = new MetricsBuilder()
                .Configuration.Configure(metricsConfigSection.AsEnumerable())
                // Adds GrafanaCloud Hosted Metrics with default options
                .Report.ToHostedMetrics(GrafanaCloudHostedMetricsUri, GrafanaCloudHostedMetricsApiKey, TimeSpan.FromSeconds(5))
                .Build();

            Reporter = Metrics.ReportRunner;
        }

        private static async Task RunUntilEscAsync(TimeSpan delayBetweenRun, CancellationTokenSource cancellationTokenSource, Func<Task> action)
        {
            Console.WriteLine("Press ESC to stop");

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                    await action();

                    Thread.Sleep(delayBetweenRun);
                }

                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false).Key;

                    if (key == ConsoleKey.Escape)
                    {
                        cancellationTokenSource.Cancel();
                        return;
                    }
                }
            }
        }
    }
}