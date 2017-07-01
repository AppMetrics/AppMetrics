// <copyright file="AppMetricsHealthAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Health;
using App.Metrics.Health.Configuration;
using App.Metrics.Health.DependencyInjection.Internal;
using App.Metrics.Health.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthAppMetricsBuilderExtensions
    {
        /// <summary>
        ///     Allows registration of health checks with the <see cref="IHealthCheckRegistry"/>.
        /// </summary>
        /// <param name="healthChecksBuilder">The <see cref="IAppMetricsHealthChecksBuilder"/> healthChecksBuilder.</param>
        /// <param name="setupAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The <see cref="IAppMetricsHealthChecksBuilder"/></returns>
        public static IAppMetricsHealthChecksBuilder AddChecks(this IAppMetricsHealthChecksBuilder healthChecksBuilder, Action<IHealthCheckRegistry> setupAction)
        {
            healthChecksBuilder.Services.TryAddSingleton<HealthCheckFluentMarkerService>();
            healthChecksBuilder.Services.Replace(ServiceDescriptor.Singleton(provider => RegisterHealthCheckRegistry(provider, setupAction)));

            return healthChecksBuilder;
        }

        // ReSharper disable UnusedMethodReturnValue.Global
        // ReSharper disable MemberCanBePrivate.Global
        internal static IAppMetricsHealthChecksBuilder AddCoreServices(this IAppMetricsHealthChecksBuilder checksBuilder)
            // ReSharper restore MemberCanBePrivate.Global
            // ReSharper restore UnusedMethodReturnValue.Global
        {
            HealthChecksAsServices.AddHealthChecksAsServices(
                checksBuilder.Services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(checksBuilder.Environment.ApplicationName));

            checksBuilder.Services.TryAddSingleton<IProvideHealth, DefaultHealthProvider>();

            return checksBuilder;
        }

        internal static IAppMetricsHealthChecksBuilder AddRequiredPlatformServices(this IAppMetricsHealthChecksBuilder checksBuilder)
        {
            checksBuilder.Services.TryAddSingleton<HealthCheckMarkerService>();
            checksBuilder.Services.AddOptions();
            checksBuilder.Services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsHealthOptions>>().Value);
            checksBuilder.Services.TryAddSingleton<IConfigureOptions<AppMetricsHealthOptions>, ConfigureAppMetricsHealthOptions>();
            checksBuilder.Services.Replace(ServiceDescriptor.Singleton(provider => RegisterHealthCheckRegistry(provider)));

            return checksBuilder;
        }

        private static IHealthCheckRegistry RegisterHealthCheckRegistry(IServiceProvider provider, Action<IHealthCheckRegistry> setupAction = null)
        {
            var logFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = logFactory.CreateLogger<HealthCheckRegistry>();

            var autoScannedHealthChecks = Enumerable.Empty<HealthCheck>();

            try
            {
                autoScannedHealthChecks = provider.GetRequiredService<IEnumerable<HealthCheck>>();
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(
                    new EventId(5000),
                    ex,
                    "Failed to load auto scanned health checks, health checks won't be registered");
            }

            var factory = new HealthCheckRegistry(autoScannedHealthChecks);
            setupAction?.Invoke(factory);
            return factory;
        }
    }
}