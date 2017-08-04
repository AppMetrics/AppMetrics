// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using App.Metrics;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MetricsSandbox
{
    public static class Host
    {
        private static readonly Random Rnd = new Random();

        public static IConfigurationRoot Configuration { get; set; }

        // public static async Task Main(string[] args)
        public static void Main(string[] args)
        {
            Init();

            IServiceCollection serviceCollection = new ServiceCollection();
            var metricsFilter = new DefaultMetricsFilter();

            ConfigureServices(serviceCollection, metricsFilter);

            var provider = serviceCollection.BuildServiceProvider();
            var metrics = provider.GetRequiredService<IMetrics>();
            var metricsProvider = provider.GetRequiredService<IProvideMetricValues>();
            var envInfoProvider = provider.GetRequiredService<EnvironmentInfoProvider>();
            var metricsOptionsAccessor = provider.GetRequiredService<IOptions<MetricsOptions>>();

            var cancellationTokenSource = new CancellationTokenSource();

            WriteEnv(envInfoProvider, metricsOptionsAccessor, cancellationTokenSource);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            RunUntilEsc(
                TimeSpan.FromSeconds(20),
                cancellationTokenSource,
                () =>
                {
                    Console.Clear();

                    RecordMetrics(metrics);

                    WriteMetrics(metricsProvider, metricsFilter, metricsOptionsAccessor, cancellationTokenSource);
                });
        }

        private static void WriteEnv(
            EnvironmentInfoProvider envInfoProvider,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            CancellationTokenSource cancellationTokenSource)
        {
            Console.WriteLine("Environment Information");
            Console.WriteLine("-------------------------------------------");

            foreach (var formatter in metricsOptionsAccessor.Value.OutputEnvFormatters)
            {
                Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                Console.WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    formatter.WriteAsync(stream, envInfoProvider.Build(), Encoding.UTF8, cancellationTokenSource.Token).GetAwaiter().GetResult();

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    Console.WriteLine(result);
                }
            }
        }

        private static void WriteMetrics(
            IProvideMetricValues metricsProvider,
            IFilterMetrics metricsFilter,
            IOptions<MetricsOptions> metricsOptionsAccessor,
            CancellationTokenSource cancellationTokenSource)
        {
            var metricsData = metricsProvider.Get(metricsFilter);

            Console.WriteLine("Metrics Formatters");
            Console.WriteLine("-------------------------------------------");

            foreach (var formatter in metricsOptionsAccessor.Value.OutputMetricsFormatters)
            {
                Console.WriteLine($"Formatter: {formatter.GetType().FullName}");
                Console.WriteLine("-------------------------------------------");

                using (var stream = new MemoryStream())
                {
                    formatter.WriteAsync(stream, metricsData, Encoding.UTF8, cancellationTokenSource.Token).GetAwaiter().GetResult();

                    var result = Encoding.UTF8.GetString(stream.ToArray());

                    Console.WriteLine(result);
                }
            }

            Console.WriteLine("Default Metrics Text Formatter");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Formatter: {metricsOptionsAccessor.Value.DefaultOutputMetricsTextFormatter}");
            Console.WriteLine("-------------------------------------------");

            using (var stream = new MemoryStream())
            {
                metricsOptionsAccessor.Value.DefaultOutputMetricsTextFormatter.WriteAsync(stream, metricsData, Encoding.UTF8, cancellationTokenSource.Token).GetAwaiter().GetResult();

                var result = Encoding.UTF8.GetString(stream.ToArray());

                Console.WriteLine(result);
            }
        }

        private static void RecordMetrics(IMetrics metrics)
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

        private static void ConfigureServices(IServiceCollection services, IFilterMetrics metricsFilter)
        {
            services.AddLogging();

            services.AddMetrics();

            // services.
            //     AddMetricsCore().
            //     AddClockType<SystemClock>().
            //     AddGlobalFilter(metricsFilter).
            //     AddDefaultReservoir(() => new DefaultAlgorithmRReservoir());
        }

        private static void Init()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            Configuration = builder.Build();
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