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
using App.Metrics.Extensions.Configuration;
using App.Metrics.Extensions.DependencyInjection;
using App.Metrics.Reporting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Console;

namespace MetricsMicrosoftExtensionsSandbox
{
    public class Host
    {
        private static readonly Random Rnd = new Random();

        private static IServiceProvider ServiceProvider { get; set; }

        private static IConfigurationRoot Configuration { get; set; }

        public static async Task Main()
        {
            Init();

            var metrics = ServiceProvider.GetRequiredService<IMetricsRoot>();
            var reporter = ServiceProvider.GetRequiredService<IRunMetricsReports>();

            var cancellationTokenSource = new CancellationTokenSource();

            await WriteEnvAsync(cancellationTokenSource, metrics);

            PressAnyKeyToContinue();

            await WriteMetricsAsync(metrics, cancellationTokenSource);

            PressAnyKeyToContinue();

            await RunUntilEscAsync(
                TimeSpan.FromSeconds(5),
                cancellationTokenSource,
                async () =>
                {
                    Clear();
                    RecordMetrics(metrics);
                    await Task.WhenAll(reporter.RunAllAsync(cancellationTokenSource.Token));
                });
        }

        private static async Task WriteEnvAsync(CancellationTokenSource cancellationTokenSource, IMetricsRoot metrics)
        {
            WriteLine("Environment Information");
            WriteLine("-------------------------------------------");

            foreach (var formatter in metrics.OutputEnvFormatters)
            {
                WriteLine($"Formatter: {formatter.GetType().FullName}");
                WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    await formatter.WriteAsync(stream, metrics.EnvironmentInfo, cancellationTokenSource.Token);

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    WriteLine(result);
                }
            }
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

        private static async Task WriteMetricsAsync(IMetricsRoot metrics, CancellationTokenSource cancellationTokenSource)
        {
            foreach (var unused in Enumerable.Range(0, 10))
            {
                RecordMetrics(metrics);
            }

            var metricsData = metrics.Snapshot.Get();

            WriteLine("Metrics Formatters");
            WriteLine("-------------------------------------------");

            foreach (var formatter in metrics.OutputMetricsFormatters)
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

        private static void RecordMetrics(IMetricsRoot metrics)
        {
            metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterOne);
            metrics.Measure.Gauge.SetValue(ApplicationsMetricsRegistry.GaugeOne, Rnd.Next(0, 100));
            metrics.Measure.Histogram.Update(ApplicationsMetricsRegistry.HistogramOne, Rnd.Next(0, 100));
            metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterOne, Rnd.Next(0, 100));

            using (metrics.Measure.Timer.Time(ApplicationsMetricsRegistry.TimerOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }

            using (metrics.Measure.Apdex.Track(ApplicationsMetricsRegistry.ApdexOne))
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

            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();
            });

            var unused = new MetricsBuilder()
                .Configuration.ReadFrom(Configuration)
                .OutputEnvInfo.AsPlainText()
                .OutputMetrics.AsPlainText()
                .OutputMetrics.AsJson()
                .Report.Using<SimpleConsoleMetricsReporter>(TimeSpan.FromSeconds(5))
                .BuildAndAddTo(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        private static async Task RunUntilEscAsync(TimeSpan delayBetweenRun, CancellationTokenSource cancellationTokenSource, Func<Task> action)
        {
            WriteLine("Press ESC to stop");

            while (true)
            {
                while (!KeyAvailable)
                {
                    await action();

                    Thread.Sleep(delayBetweenRun);
                }

                while (KeyAvailable)
                {
                    var key = ReadKey(false).Key;

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