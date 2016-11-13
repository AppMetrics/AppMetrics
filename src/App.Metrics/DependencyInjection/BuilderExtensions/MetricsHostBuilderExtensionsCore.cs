// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Serialization;
using App.Metrics.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsHostBuilderExtensionsCore
    {
        public static IMetricsHostBuilder AddRequiredPlatformServices(this IMetricsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
            builder.Services.AddOptions();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsOptions>>().Value);

            return builder;
        }

        internal static void AddCoreServices(this IMetricsHostBuilder builder)
        {
            builder.Services.TryAddTransient<Func<string, IMetricContextRegistry>>(
                provider => { return context => new DefaultMetricContextRegistry(context); });
            builder.Services.TryAddSingleton<IClock, StopwatchClock>();
            builder.Services.TryAddSingleton<IMetricsFilter, DefaultMetricsFilter>();
            builder.Services.TryAddSingleton<EnvironmentInfoBuilder, EnvironmentInfoBuilder>();
            builder.Services.TryAddSingleton<IMetricDataSerializer, NullMetricDataSerializer>();
            builder.Services.TryAddSingleton<IHealthStatusSerializer, NullHealthStatusSerializer>();
            builder.Services.TryAddSingleton<IAdvancedMetrics, DefaultAdvancedMetrics>();
            builder.Services.TryAddSingleton<IMetricsRegistry>(provider =>
            {
                var options = provider.GetRequiredService<AppMetricsOptions>();

                if (!options.MetricsEnabled)
                {
                    return new NullMetricsRegistry();
                }

                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var clock = provider.GetRequiredService<IClock>();
                var envBuilder = provider.GetRequiredService<EnvironmentInfoBuilder>();
                var newContextRegistry = provider.GetRequiredService<Func<string, IMetricContextRegistry>>();

                return new DefaultMetricsRegistry(loggerFactory, options, clock, envBuilder, newContextRegistry);
            });

            builder.Services.TryAddSingleton<IMetrics, DefaultMetrics>();
            builder.Services.TryAddSingleton(provider => builder.Environment);
        }
    }
}