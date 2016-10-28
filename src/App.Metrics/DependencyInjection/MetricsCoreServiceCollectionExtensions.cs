// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Json;
using App.Metrics.Utils;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions
// ReSharper restore CheckNamespace
{
    internal static class MetricsCoreServiceCollectionExtensions
    {
        private static readonly IReadOnlyDictionary<JsonSchemeVersion, Type> MetricsJsonBuilderVersionMapping =
            new ReadOnlyDictionary<JsonSchemeVersion, Type>(new Dictionary<JsonSchemeVersion, Type>
            {
                { JsonSchemeVersion.AlwaysLatest, typeof(MetricsJsonBuilderV1) },
                { JsonSchemeVersion.Version1, typeof(MetricsJsonBuilderV1) }
            });

        internal static void AddDefaultJsonServices(this IServiceCollection services)
        {
            services.TryAddSingleton<MetricsJsonBuilderV1, MetricsJsonBuilderV1>();
            services.TryAddSingleton(typeof(IMetricsJsonBuilder), provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppMetricsOptions>>();
                var jsonBuilderType = MetricsJsonBuilderVersionMapping[options.Value.JsonSchemeVersion];
                return provider.GetRequiredService(jsonBuilderType);
            });
        }

        internal static IMetricsHost AddMetricsCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return AddMetricsCore(services, setupAction: null, metricsContext: default(IMetricsContext));
        }

        internal static IMetricsHost AddMetricsCore(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction,
            IMetricsContext metricsContext)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var metricsEnvironment = new MetricsAppEnvironment(PlatformServices.Default.Application);

            services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();

            services.ConfigureDefaultServices();
            services.AddDefaultJsonServices();
            services.AddMetricsCoreServices(metricsEnvironment, metricsContext);

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new MetricsHost(services, metricsEnvironment);
        }

        internal static void AddMetricsCoreServices(this IServiceCollection services,
            IMetricsEnvironment environment, IMetricsContext metricsContext)
        {
            services.TryAddTransient<Func<string, IMetricGroupRegistry>>(provider => { return group => new DefaultMetricGroupRegistry(group); });
            services.TryAddSingleton<IMetricsBuilder, DefaultMetricsBuilder>();
            services.TryAddSingleton<IMetricsRegistry, DefaultMetricsRegistry>();
            services.TryAddSingleton<IMetricsDataManager, DefaultMetricsDataManager>();
            services.TryAddSingleton(typeof(IClock), provider => provider.GetRequiredService<IOptions<AppMetricsOptions>>().Value.Clock);
            services.TryAddSingleton<EnvironmentInfoBuilder, EnvironmentInfoBuilder>();

            if (metricsContext == null)
            {
                services.TryAddSingleton<IMetricsContext, DefaultMetricsContext>();
            }
            else
            {
                services.TryAddSingleton(metricsContext);
            }

            services.TryAddSingleton(provider => environment);
        }

        private static void ConfigureDefaultServices(this IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}