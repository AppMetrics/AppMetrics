// <copyright file="MetricsHostExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using MetricsHostingMicrosoftExtensionsSandbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using static System.Console;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Hosting
// ReSharper restore CheckNamespace
{
    public static class MetricsHostExtensions
    {
        private static readonly Random Rnd = new Random();

        public static void RecordMetrics(this IHost host, IMetricsRoot metrics)
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

        public static async Task WriteEnvAsync(this IHost host, CancellationTokenSource cancellationTokenSource, IMetricsRoot metrics)
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

        public static async Task WriteMetricsAsync(this IHost host, IMetricsRoot metrics, CancellationTokenSource cancellationTokenSource)
        {
            foreach (var unused in Enumerable.Range(0, 10))
            {
                host.RecordMetrics(metrics);
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

        public static Task WriteHealthChecksAsync(this IHost host, CancellationTokenSource cancellationTokenSource)
        {
            WriteLine("Health Checks");
            WriteLine("-------------------------------------------");

            foreach (var formatter in host.Services.GetService<IOptions<HealthCheckServiceOptions>>().Value.Registrations)
            {
                WriteLine($"Health Check: {formatter.Name}");
                WriteLine("\tTags: " + string.Join(", ", formatter.Tags));
            }

            return Task.CompletedTask;
        }
    }
}