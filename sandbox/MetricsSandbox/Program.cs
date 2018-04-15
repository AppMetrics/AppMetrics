// <copyright file="Program.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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

        private static IRunMetricsReports Reporter { get; set; }

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.LiterateConsole(LogEventLevel.Information)
                         .WriteTo.Seq("http://localhost:5341", LogEventLevel.Verbose)
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
                               var dataKeys = new GeneratedMetricNameMapping();

                               if (FilterMetricValueTypes)
                               {
                                   dataKeys.Apdex.Remove(ApdexValueDataKeys.Samples);
                                   dataKeys.Counter.Remove(CounterValueDataKeys.Total);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.Samples);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.Count);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.Sum);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.Min);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.Max);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.Mean);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.Median);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.StdDev);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.P999);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.P99);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.P98);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.P95);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.P75);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.UserLastValue);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.UserMinValue);
                                   dataKeys.Histogram.Remove(HistogramValueDataKeys.UserMaxValue);
                                   dataKeys.Meter.Remove(MeterValueDataKeys.Count);
                                   dataKeys.Meter.Remove(MeterValueDataKeys.Rate1M);
                                   dataKeys.Meter.Remove(MeterValueDataKeys.Rate15M);
                                   dataKeys.Meter.Remove(MeterValueDataKeys.RateMean);
                                   dataKeys.Meter.Remove(MeterValueDataKeys.SetItemPercent);
                               }

                               var metricsConfigSection = hostContext.Configuration.GetSection(nameof(MetricsOptions));
                               // Metrics = AppMetrics.CreateDefaultBuilder() to use the default configuration
                               Metrics = new MetricsBuilder()
                                                    .Configuration.Configure(
                                                        options =>
                                                        {
                                                            options.AddServerTag();
                                                            options.AddAppTag();
                                                            options.AddEnvTag();
                                                        })
                                                   .Configuration.Configure(metricsConfigSection.AsEnumerable())
                                                   .OutputEnvInfo.AsPlainText()
                                                   .OutputMetrics.AsPlainText(
                                                       options =>
                                                       {
                                                           options.DataKeys = dataKeys;
                                                       })
                                                   .SampleWith.ForwardDecaying()
                                                   .TimeWith.StopwatchClock()
                                                   .Report.Using<SimpleConsoleMetricsReporter>(TimeSpan.FromSeconds(2))
                                                   .Build();
                               Reporter = Metrics.ReportRunner;
                           })
                .Build();

            var cancellationTokenSource = new CancellationTokenSource();

            await WriteEnvAsync(cancellationTokenSource);

            PressAnyKeyToContinue();

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
            Metrics.Measure.Counter.Increment(ApplicationsMetricsRegistry.CounterWithSetItems, "item1");
            Metrics.Measure.Gauge.SetValue(ApplicationsMetricsRegistry.GaugeOne, Rnd.Next(0, 100));
            Metrics.Measure.Histogram.Update(ApplicationsMetricsRegistry.HistogramOne, Rnd.Next(0, 100));
            Metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterOne, Rnd.Next(0, 100));
            Metrics.Measure.Meter.Mark(ApplicationsMetricsRegistry.MeterWithSetItems, Rnd.Next(0, 100), "item1");

            using (Metrics.Measure.Timer.Time(ApplicationsMetricsRegistry.TimerOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }

            using (Metrics.Measure.Apdex.Track(ApplicationsMetricsRegistry.ApdexOne))
            {
                Thread.Sleep(Rnd.Next(0, 100));
            }
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