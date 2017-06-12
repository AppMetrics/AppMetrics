// <copyright file="MetricsHostBuilderHealthExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using App.Metrics.Core.DependencyInjection.Internal;
using App.Metrics.Health;
using App.Metrics.Health.DependencyInjection.Internal;
using App.Metrics.Health.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace

    public static class MetricsHostBuilderHealthExtensions
    {
        public static IMetricsHostBuilder AddHealthChecks(this IMetricsHostBuilder builder)
        {
            builder.AddHealthChecks(setupAction: null);
            return builder;
        }

        // ReSharper disable UnusedMethodReturnValue.Global
        // ReSharper disable MemberCanBePrivate.Global
        public static IMetricsHostBuilder AddHealthChecks(this IMetricsHostBuilder builder, Action<IHealthCheckFactory> setupAction)
            // ReSharper restore MemberCanBePrivate.Global
            // ReSharper restore UnusedMethodReturnValue.Global
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddSingleton<HealthCheckMarkerService, HealthCheckMarkerService>();
            HealthChecksAsServices.AddHealthChecksAsServices(
                builder.Services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(builder.Environment.ApplicationName));

            builder.Services.TryAddSingleton<DefaultHealthProvider>();
            builder.Services.Replace(ServiceDescriptor.Singleton<IProvideHealth>(provider => provider.GetRequiredService<DefaultHealthProvider>()));

            builder.Services.Replace(
                ServiceDescriptor.Singleton<IHealthCheckFactory>(
                    provider =>
                    {
                        var logFactory = provider.GetRequiredService<ILoggerFactory>();
                        var logger = logFactory.CreateLogger<HealthCheckFactory>();
                        var metrics = provider.GetRequiredService<Lazy<IMetrics>>();

                        var autoScannedHealthChecks = Enumerable.Empty<HealthCheck>();

                        try
                        {
                            autoScannedHealthChecks = provider.GetRequiredService<IEnumerable<HealthCheck>>();
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(
                                new EventId(5000),
                                ex,
                                "Failed to load auto scanned health checks, health checks won't be registered");
                        }

                        var factory = new HealthCheckFactory(logger, metrics, autoScannedHealthChecks);
                        setupAction?.Invoke(factory);
                        return factory;
                    }));

            return builder;
        }
    }
}