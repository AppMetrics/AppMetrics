// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static System.Console;

namespace HealthMicrosoftExtensionsSandbox
{
    public class Host
    {
        private static IServiceProvider ServiceProvider { get; set; }

        private static IConfigurationRoot Configuration { get; set; }

        public static async Task Main()
        {
            Init();

            var health = ServiceProvider.GetRequiredService<IHealthRoot>();

            var cancellationTokenSource = new CancellationTokenSource();

            await RunUntilEscAsync(
                TimeSpan.FromSeconds(10),
                cancellationTokenSource,
                async () =>
                {
                    Clear();

                    var healthStatus = await health.HealthCheckRunner.ReadAsync(cancellationTokenSource.Token);

                    foreach (var formatter in health.OutputHealthFormatters)
                    {
                        WriteLine($"Formatter: {formatter.GetType().FullName}");
                        WriteLine("-------------------------------------------");

                        using (var stream = new MemoryStream())
                        {
                            await formatter.WriteAsync(stream, healthStatus, cancellationTokenSource.Token);

                            var result = Encoding.UTF8.GetString(stream.ToArray());

                            WriteLine(result);
                        }
                    }
                });
        }

        private static void Init()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            var unused = new HealthBuilder()
                .Configuration.ReadFrom(Configuration)
                .OutputHealth.AsPlainText()
                .OutputHealth.AsJson()
                .HealthChecks.AddCheck("inline-check", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                .HealthChecks.RegisterFromAssembly(services)
                .BuildAndAddTo(services);

            // BuildAndAddTo will add services to the IServiceCollection
            // services.AddHealth(health);

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();
            });

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