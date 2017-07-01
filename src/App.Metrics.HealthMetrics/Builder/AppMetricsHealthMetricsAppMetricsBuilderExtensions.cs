// <copyright file="AppMetricsHealthMetricsAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Health.DependencyInjection.Internal;
using App.Metrics.Health.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
// ReSharper restore CheckNamespace
{
    public static class AppMetricsHealthMetricsAppMetricsBuilderExtensions
    {
        /// <summary>
        ///     Allows registration of health checks using metric values.
        /// </summary>
        /// <param name="healthChecksBuilder">The <see cref="IAppMetricsHealthChecksBuilder"/> healthChecksBuilder.</param>
        /// <param name="setupAction">The <see cref="IHealthCheckRegistry"/> setup action allowing health checks to be regsitered.</param>
        /// <returns>The <see cref="IAppMetricsHealthChecksBuilder"/></returns>
        public static IAppMetricsHealthChecksBuilder AddChecks(this IAppMetricsHealthChecksBuilder healthChecksBuilder, Action<IHealthCheckRegistry, IMetrics> setupAction)
        {
            healthChecksBuilder.Services.Replace(ServiceDescriptor.Singleton(provider => RegisterHealthCheckRegistry(provider, setupAction)));

            return healthChecksBuilder;
        }

        private static IHealthCheckRegistry RegisterHealthCheckRegistry(IServiceProvider provider, Action<IHealthCheckRegistry, IMetrics> setupAction)
        {
            // TODO: 2.0.0 don't want replace the Health Registry instance, end up loosing health registered in AddChecks without metrics.
            // Throwing an exception is this case is an ugly workaround for now.
            HealthServicesHelper.ThrowIfHealthAddChecksHasAlreadyBeenCalled(provider);

            var logFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = logFactory.CreateLogger<HealthCheckRegistry>();
            var metrics = provider.GetRequiredService<IMetrics>();

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
            setupAction?.Invoke(factory, metrics);
            return factory;
        }
    }
}
