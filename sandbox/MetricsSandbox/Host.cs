// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Infrastructure;
using App.Metrics.ReservoirSampling.Uniform;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsSandbox
{
    public static class Host
    {
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

            var cancellationTokenSource = new CancellationTokenSource();

            RunUntilEsc(
                TimeSpan.FromSeconds(10),
                cancellationTokenSource,
                () =>
                {
                    Console.Clear();

                    metrics.Measure.Counter.Increment(CounterRegistry.CounterOne);

                    foreach (var contenxt in metricsProvider.Get(metricsFilter).Contexts)
                    {
                        foreach (var counterValueSource in contenxt.Counters)
                        {
                            Console.WriteLine("COUNTERS");
                            Console.WriteLine($"{counterValueSource.Name} Count: {counterValueSource.Value.Count}");
                        }
                    }
                });
        }

        private static void ConfigureServices(IServiceCollection services, IFilterMetrics metricsFilter)
        {
            services.AddLogging();

            // services.AddMetrics();

            services.
                AddMetricsCore().
                AddClockType<SystemClock>().
                AddGlobalFilter(metricsFilter).
                AddDefaultReservoir(() => new DefaultAlgorithmRReservoir());
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