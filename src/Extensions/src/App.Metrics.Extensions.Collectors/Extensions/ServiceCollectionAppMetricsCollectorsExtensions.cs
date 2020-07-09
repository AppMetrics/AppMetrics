// <copyright file="ServiceCollectionAppMetricsHealthCheckPublishingExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Extensions.Collectors;
using App.Metrics.Extensions.Collectors.HostedServices;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class ServiceCollectionAppMetricsCollectorsExtensions
    {
        /// <summary>
        /// Adds a hosted service which collects system metrics such as CPU usage and memory working set.
        /// </summary>
        public static IServiceCollection AddAppMetricsSystemMetricsCollector(
            this IServiceCollection services,
            Action<MetricsSystemUsageCollectorOptions> optionsSetup = null)
        {
            var options = new MetricsSystemUsageCollectorOptions();
            optionsSetup?.Invoke(options);
            services.AddSingleton(options);
            
            return services.AddHostedService<SystemUsageCollectorHostedService>();
        }

        /// <summary>
        /// Adds a hosted service which collects GC events using the "Microsoft-Windows-DotNETRuntime" EventSource.
        /// </summary>
        public static IServiceCollection AddAppMetricsGcEventsMetricsCollector(
            this IServiceCollection services,
            Action<MetricsGcEventsCollectorOptions> optionsSetup = null)
        {
            var options = new MetricsGcEventsCollectorOptions();
            optionsSetup?.Invoke(options);
            services.AddSingleton(options);
            
            return services.AddHostedService<GcEventsCollectorHostedService>();
        }

        /// <summary>
        /// Adds hosted services to collect system usage and gc event metrics
        /// </summary>
        public static IServiceCollection AddAppMetricsCollectors(
            this IServiceCollection services,
            Action<MetricsSystemUsageCollectorOptions> systemUsageOptionsSetup = null,
            Action<MetricsGcEventsCollectorOptions> gcEventsOptionsSetup = null)
        {
            services.AddAppMetricsSystemMetricsCollector(systemUsageOptionsSetup);
            services.AddAppMetricsGcEventsMetricsCollector(gcEventsOptionsSetup);

            return services;
        }
    }
}