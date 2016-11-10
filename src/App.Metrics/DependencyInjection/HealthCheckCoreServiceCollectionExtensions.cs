// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;

namespace App.Metrics.DependencyInjection
{
    internal static class HealthCheckCoreServiceCollectionExtensions
    {
        internal static void AddDefaultHealthCheckServices(this IServiceCollection services)
        {
            //TODO: AH - this is already instantiated as part of caore metrics
            var metricsEnvironment = new MetricsAppEnvironment(PlatformServices.Default.Application);

            services.AddHealthChecks(metricsEnvironment);
        }

        internal static IMetricsHostBuilder AddMetricsHealthCheckCore(
            this IMetricsHostBuilder host,
            Action<IHealthCheckFactory> registerHealthChecks)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.Services.TryAddSingleton<HealthCheckMarkerService, HealthCheckMarkerService>();

            host.Services.AddMetricsHealthCheckCoreServices(registerHealthChecks);
            host.Services.AddDefaultHealthCheckServices();

            return host;
        }

        internal static IMetricsHostBuilder AddMetricsHealthCheckCore(this IMetricsHostBuilder host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            return AddMetricsHealthCheckCore(host, registerHealthChecks: null);
        }

        private static void AddMetricsHealthCheckCoreServices(this IServiceCollection services, Action<IHealthCheckFactory> registerHealthChecks)
        {
            services.TryAddTransient<IHealthCheckFactory>(provider =>
            {
                var autoScannedHealthChecks = provider.GetRequiredService<IEnumerable<HealthCheck>>();
                var factory = new HealthCheckFactory(autoScannedHealthChecks);
                registerHealthChecks?.Invoke(factory);
                return factory;
            });
        }
    }
}