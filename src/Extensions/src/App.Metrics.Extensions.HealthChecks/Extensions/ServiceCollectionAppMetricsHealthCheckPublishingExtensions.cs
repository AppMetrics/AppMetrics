// <copyright file="ServiceCollectionAppMetricsHealthCheckPublishingExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>
using App.Metrics.Extensions.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class ServiceCollectionAppMetricsHealthCheckPublishingExtensions
    {
        /// <summary>
        /// Registers a <see cref="IHealthCheckPublisher"/>. See <see href="https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-2.2#health-check-publisher"/> for information on how to setup the background service
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The same service collection</returns>
        public static IServiceCollection AddAppMetricsHealthPublishing(this IServiceCollection services)
        {
            return services.AddSingleton<IHealthCheckPublisher, AppMetricsHealthCheckPublisher>();
        }
    }
}
