// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using static System.Console;

namespace MetricsHostingMicrosoftExtensionsSandbox
{
    public class Host
    {
        private const string HealthCheckServiceAssembly =
            "Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckPublisherHostedService";

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
                 .ConfigureMetrics(
                    (context, builder) =>
                    {
                        builder.Report.Using<SimpleConsoleMetricsReporter>(TimeSpan.FromSeconds(5));
                    })
                 .ConfigureServices(services =>
                 {
                     services.AddHealthChecks()
                         .AddCheck(
                             "example_health_check_healthy",
                             new StaticHealthCheck(HealthCheckResult.Healthy()),
                             failureStatus: HealthStatus.Degraded,
                             tags: new[] { "ready" });
                     services.AddHealthChecks()
                         .AddCheck(
                             "example_health_check_degraded",
                             new StaticHealthCheck(HealthCheckResult.Degraded()),
                             failureStatus: HealthStatus.Degraded,
                             tags: new[] { "ready" });

                     // Won't be required for ASP.NET Core 3.0 (https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2#health-check-publisher)
                     services.TryAddEnumerable(
                         ServiceDescriptor.Singleton(
                             typeof(IHostedService),
                             typeof(HealthCheckPublisherOptions).Assembly
                                 .GetType(HealthCheckServiceAssembly)));

                     services.Configure<HealthCheckPublisherOptions>(options =>
                     {
                         options.Delay = TimeSpan.FromSeconds(2);
                         options.Predicate = (check) => check.Tags.Contains("ready");
                     });

                     services.AddAppMetricsHealthPublishing();
                 })
                .Build();

            var metrics = host.Services.GetRequiredService<IMetricsRoot>();

            var cancellationTokenSource = new CancellationTokenSource();

            await host.WriteEnvAsync(cancellationTokenSource, metrics);

            host.PressAnyKeyToContinue();

            await host.WriteMetricsAsync(metrics, cancellationTokenSource);

            host.PressAnyKeyToContinue();

            await host.WriteHealthChecksAsync(cancellationTokenSource);

            host.PressAnyKeyToContinue();

            var recordMetricsTask = new AppMetricsTaskScheduler(
                TimeSpan.FromSeconds(2),
                () =>
                {
                    Clear();
                    host.RecordMetrics(metrics);
                    return Task.CompletedTask;
                });

            recordMetricsTask.Start();

            await host.RunAsync(token: cancellationTokenSource.Token);
        }
    }
}