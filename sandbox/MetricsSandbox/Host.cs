// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace MetricsSandbox
{
    public static class Host
    {
        private static readonly Random Rnd = new Random();

        private static IConfigurationRoot Configuration { get; set; }

        private static IMetricsRoot Metrics { get; set; }

        public static async Task Main()
        {
            Init();

            var cancellationTokenSource = new CancellationTokenSource();

            await WriteEnvAsync(cancellationTokenSource);

            Console.ReadKey();

            RunUntilEsc(
                TimeSpan.FromSeconds(5),
                cancellationTokenSource,
                async () =>
                {
                    Console.Clear();

                    RecordMetrics();

                    await WriteMetricsAsync(cancellationTokenSource);
                });
        }

        private static async Task WriteEnvAsync(CancellationTokenSource cancellationTokenSource)
        {
            Console.WriteLine("Environment Information");
            Console.WriteLine("-------------------------------------------");

            foreach (var formatter in Metrics.OutputEnvFormatters)
            {
                Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                Console.WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    await formatter.WriteAsync(stream, Metrics.EnvironmentInfo, cancellationTokenSource.Token);

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    Console.WriteLine(result);
                }
            }
        }

        private static async Task WriteMetricsAsync(CancellationTokenSource cancellationTokenSource)
        {
            var metricsData = Metrics.Snapshot.Get();

            Console.WriteLine("Metrics Formatters");
            Console.WriteLine("-------------------------------------------");

            foreach (var formatter in Metrics.OutputMetricsFormatters)
            {
                Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                Console.WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    await formatter.WriteAsync(stream, metricsData, cancellationTokenSource.Token);

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    Console.WriteLine(result);
                }
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
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Metrics = AppMetrics.CreateDefaultBuilder().Build();
        }

        private static void RunUntilEsc(TimeSpan delayBetweenRun, CancellationTokenSource cancellationTokenSource, Action action)
        {
            Console.WriteLine("Press ESC to stop");

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                    action();

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