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
using static System.Console;

namespace MetricsInfluxDBSandbox
{
    public static class Host
    {
        private const string InfluxDbDatabase = "metricsinfluxdbsandboxconsole";
        private const string InfluxDbUri = "http://127.0.0.1:32768";
        private static readonly bool FilterMetricFields = false;
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
                    Clear();
                    RecordMetrics();
                    await Task.WhenAll(Reporter.RunAllAsync(cancellationTokenSource.Token));
                });
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

        private static async Task WriteMetricsAsync(CancellationTokenSource cancellationTokenSource)
        {
            foreach (var unused in Enumerable.Range(0, 10))
            {
                RecordMetrics();
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
                .MetricFields.Configure(
                          fields =>
                          {
                              if (FilterMetricFields)
                              {
                                  fields.Meter.OnlyInclude(MeterFields.Rate1M);
                                  fields.Apdex.Exclude();
                                  fields.Counter.OnlyInclude(CounterFields.Value);
                                  fields.Gauge.Exclude();
                                  fields.Histogram.Exclude();
                              }
                          })
                // Adds LineProtocolFormatter with default options, can override fields reported to influx using fields => fields...
                .Report.ToInfluxDb(InfluxDbUri, InfluxDbDatabase, TimeSpan.FromSeconds(5))
                .Build();

            Reporter = Metrics.ReportRunner;
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