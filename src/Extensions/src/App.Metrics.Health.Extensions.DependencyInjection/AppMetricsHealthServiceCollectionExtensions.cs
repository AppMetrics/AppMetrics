// <copyright file="AppMetricsHealthServiceCollectionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Extensions.DependencyInjection.Internal;
using App.Metrics.Health.Formatters;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods on <see cref="IServiceCollection" /> to register App Metrics Health services via an
    ///     <see cref="IHealthBuilder" />.
    /// </summary>
    public static class AppMetricsHealthServiceCollectionExtensions
    {
        public static IServiceCollection AddHealth(this IServiceCollection services, Action<IHealthBuilder> setupHealth)
        {
            var builder = new HealthBuilder();
            setupHealth(builder);

            return AddHealth(services, builder);
        }

        public static IServiceCollection AddHealth(this IServiceCollection services, IHealthBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var health = builder.Build();

            AddCoreServices(services, health);

            return services;
        }

        public static IServiceCollection AddHealth(this IServiceCollection services)
        {
            var builder = AppMetricsHealth.CreateDefaultBuilder();

            return services.AddHealth(builder);
        }

        public static IServiceCollection AddHealth(this IServiceCollection services, IHealthRoot health)
        {
            if (health == null)
            {
                throw new ArgumentNullException(nameof(health));
            }

            AddCoreServices(services, health);

            return services;
        }

        internal static void AddCoreServices(IServiceCollection services, IHealthRoot health)
        {
            services.TryAddSingleton<IReadOnlyCollection<IHealthOutputFormatter>>(provider => health.OutputHealthFormatters);
            services.TryAddSingleton<IHealth>(health);
            services.TryAddSingleton<IHealthRoot>(health);
            services.TryAddSingleton<HealthOptions>(health.Options);
            services.TryAddSingleton<IRunHealthChecks>(health.HealthCheckRunner);
            services.TryAddSingleton<AppMetricsHealthMarkerService, AppMetricsHealthMarkerService>();
        }
    }
}