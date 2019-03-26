// <copyright file="ServiceCollectionHealthReportingExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.Hosting;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class ServiceCollectionHealthReportingExtensions
    {
        public static IServiceCollection AddHealthReportingHostedService(
            this IServiceCollection services,
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler = null)
        {
            return services.AddHealthReportingHostedService(TimeSpan.FromSeconds(10), unobservedTaskExceptionHandler);
        }

        public static IServiceCollection AddHealthReportingHostedService(
            this IServiceCollection services,
            TimeSpan checkInterval,
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler = null)
        {
            services.AddSingleton<IHostedService, HealthReporterBackgroundService>(serviceProvider =>
            {
                var health = serviceProvider.GetRequiredService<IHealthRoot>();

                var instance = new HealthReporterBackgroundService(health, checkInterval);

                if (unobservedTaskExceptionHandler != null)
                {
                    instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                }

                return instance;
            });

            return services;
        }
    }
}
