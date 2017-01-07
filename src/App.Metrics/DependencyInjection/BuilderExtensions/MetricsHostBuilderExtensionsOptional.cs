// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Internal;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Interfaces;
using App.Metrics.Reporting.Internal;
using App.Metrics.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsHostBuilderExtensionsOptional
    {
        public static IMetricsHostBuilder AddClockType<T>(this IMetricsHostBuilder builder)
            where T : class, IClock, new()
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IClock>(new T()));

            return builder;
        }

        public static IMetricsHostBuilder AddGlobalFilter(this IMetricsHostBuilder builder, IMetricsFilter filter)
        {
            builder.Services.Replace(ServiceDescriptor.Singleton(filter));

            return builder;
        }

        public static IMetricsHostBuilder AddHealthChecks(this IMetricsHostBuilder builder)
        {
            builder.AddHealthChecks(setupAction: null);
            return builder;
        }

        public static IMetricsHostBuilder AddHealthChecks(this IMetricsHostBuilder builder, Action<IHealthCheckFactory> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.TryAddSingleton<HealthCheckMarkerService, HealthCheckMarkerService>();
            HealthChecksAsServices.AddHealthChecksAsServices(builder.Services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(builder.Environment.ApplicationName));

            builder.Services.Replace(ServiceDescriptor.Singleton<IHealthCheckFactory>(provider =>
            {
                var autoScannedHealthChecks = provider.GetRequiredService<IEnumerable<HealthCheck>>();
                var logFactory = provider.GetRequiredService<ILoggerFactory>();
                var logger = logFactory.CreateLogger<HealthCheckFactory>();
                var factory = new HealthCheckFactory(logger, autoScannedHealthChecks);
                setupAction?.Invoke(factory);
                return factory;
            }));

            return builder;
        }

        public static IMetricsHostBuilder AddReporting(this IMetricsHostBuilder builder, Action<IReportFactory> setupAction)
        {
            builder.Services.TryAddSingleton<AppMetricsReportingMarkerService, AppMetricsReportingMarkerService>();

            builder.Services.Replace(ServiceDescriptor.Singleton<IReportFactory>(provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var metrics = provider.GetRequiredService<IMetrics>();
                var options = provider.GetRequiredService<AppMetricsOptions>();

                if (!options.ReportingEnabled || setupAction == null)
                {
                    return new NoOpReportFactory();
                }

                var factory = new ReportFactory(metrics, loggerFactory);
                setupAction.Invoke(factory);
                return factory;
            }));

            return builder;
        }
    }
}