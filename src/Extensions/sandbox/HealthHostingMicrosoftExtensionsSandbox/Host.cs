// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using static System.Console;

namespace HealthHostingMicrosoftExtensionsSandbox
{
    public class Host
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.LiterateConsole(LogEventLevel.Information)
                         .WriteTo.Seq("http://localhost:5341", LogEventLevel.Verbose)
                         .CreateLogger();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var metrics = AppMetrics.CreateDefaultBuilder()
                .Configuration.ReadFrom(configuration)
                .Report.Using<SimpleConsoleMetricsReporter>()
                .Build();

            var host = new HostBuilder()
                .ConfigureAppConfiguration(
                           builder =>
                           {
                               builder.SetBasePath(Directory.GetCurrentDirectory())
                                      .AddEnvironmentVariables()
                                      .AddJsonFile("appsettings.json", optional: true)
                                      .AddCommandLine(args);
                           })
                .ConfigureMetrics(metrics)
                .ConfigureHealthWithDefaults(
                    (context, services, builder) =>
                    {
                        builder.OutputHealth.AsPlainText()
                               .OutputHealth.AsJson()
                               .HealthChecks.AddCheck("inline-check", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                               .Report.ToMetrics(metrics);
                    })
                .Build();

            var health = host.Services.GetRequiredService<IHealthRoot>();
            var cancellationTokenSource = new CancellationTokenSource();

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

            WriteLine("Press any key to continue..");
            ReadKey();

            await host.RunAsync(token: cancellationTokenSource.Token);
        }
    }
}