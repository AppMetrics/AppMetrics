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

            services.TryAddSingleton<IHealthCheckManager, DefaultHealthCheckManager>();

            services.AddHealthChecks(metricsEnvironment);
        }

        internal static IMetricsHostBuilder AddMetricsHealthCheckCore(
            this IMetricsHostBuilder host,
            Action<AppMetricsHealthCheckOptions> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.Services.TryAddSingleton<HealthCheckMarkerService, HealthCheckMarkerService>();

            host.Services.AddMetricsHealthCheckCoreServices();
            host.Services.AddDefaultHealthCheckServices();

            if (setupAction != null)
            {
                host.Services.Configure(setupAction);
            }

            return host;
        }

        internal static IMetricsHostBuilder AddMetricsHealthCheckCore(this IMetricsHostBuilder host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            return AddMetricsHealthCheckCore(host, setupAction: null);
        }

        private static void AddMetricsHealthCheckCoreServices(this IServiceCollection services)
        {
            services.TryAddTransient<Func<IReadOnlyDictionary<string, HealthCheck>>>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppMetricsHealthCheckOptions>>();
                var autoScannedHealthChecks = provider.GetRequiredService<IEnumerable<HealthCheck>>();
                var factory = new HealthCheckFactory(autoScannedHealthChecks);
                options.Value.HealthChecks(factory);
                return () => factory.Checks;
            });
        }
    }
}