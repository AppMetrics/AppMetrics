// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions
// ReSharper restore CheckNamespace
{
    internal static class MetricsCoreServiceCollectionExtensions
    {
        internal static IMetricsHostBuilder AddMetricsCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return AddMetricsCore(services, setupAction: null, metrics: default(IMetrics));
        }

        internal static IMetricsHostBuilder AddMetricsCore(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction,
            IMetrics metrics)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var metricsEnvironment = new MetricsAppEnvironment(PlatformServices.Default.Application);

            services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();

            services.ConfigureDefaultServices();
            services.AddMetricsCoreServices(metricsEnvironment, metrics);

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new MetricsHostBuilder(services, metricsEnvironment);
        }

        internal static void AddMetricsCoreServices(this IServiceCollection services,
            IMetricsEnvironment environment, IMetrics metrics)
        {
            services.TryAddTransient<Func<string, IMetricContextRegistry>>(provider => { return context => new DefaultMetricContextRegistry(context); });
            services.TryAddSingleton(provider => provider.GetRequiredService<IOptions<AppMetricsOptions>>().Value);
            services.TryAddSingleton(provider => provider.GetRequiredService<IOptions<AppMetricsOptions>>().Value.Clock);
            services.TryAddSingleton<IMetricsRegistry, DefaultMetricsRegistry>();
            services.TryAddSingleton<IMetricsDataManager, DefaultMetricsDataManager>();
            services.TryAddSingleton<EnvironmentInfoBuilder, EnvironmentInfoBuilder>();
            services.TryAddSingleton<IMetricDataSerializer, NullMetricDataSerializer>();
            services.TryAddSingleton<IHealthStatusSerializer, NullHealthStatusSerializer>();
            services.TryAddSingleton<IAdvancedMetrics, DefaultAdancedMetrics>();

            if (metrics == null)
            {
                services.TryAddSingleton<IMetrics, DefaultMetrics>();
            }
            else
            {
                services.TryAddSingleton(metrics);
            }

            services.TryAddSingleton(provider => environment);
        }

        private static void ConfigureDefaultServices(this IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}