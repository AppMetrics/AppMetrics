// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Internal.Managers;
using App.Metrics.Reporting.Interfaces;
using App.Metrics.Reporting.Internal;
using App.Metrics.Serialization;
using App.Metrics.Serialization.Interfaces;
using App.Metrics.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace
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
                provider =>
                {
                    var globalTags = provider.GetRequiredService<AppMetricsOptions>().GlobalTags;
                    return context => new DefaultMetricContextRegistry(context, new GlobalMetricTags(globalTags));
                });
            builder.Services.TryAddSingleton<IReportFactory, NoOpReportFactory>();
            builder.Services.TryAddSingleton<IHealthCheckFactory, NoOpHealthCheckFactory>();
            builder.Services.TryAddSingleton<IClock, StopwatchClock>();
            builder.Services.TryAddSingleton<IMetricsFilter, DefaultMetricsFilter>();
            builder.Services.TryAddSingleton<EnvironmentInfoProvider, EnvironmentInfoProvider>();
            builder.Services.TryAddSingleton<IMetricDataSerializer, NoOpMetricDataSerializer>();
            builder.Services.TryAddSingleton<IHealthStatusSerializer, NoOpHealthStatusSerializer>();
            builder.Services.TryAddSingleton<IMetricsManagerFactory, DefaultMetricsManagerFactory>();
            builder.Services.TryAddSingleton<IAdvancedMetrics, DefaultAdvancedMetrics>();
            builder.Services.TryAddSingleton<IMetricsRegistry>(
                provider =>
                {
                    var options = provider.GetRequiredService<AppMetricsOptions>();

                    if (!options.MetricsEnabled)
                    {
                        return new NullMetricsRegistry();
                    }

                    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                    var clock = provider.GetRequiredService<IClock>();
                    var envBuilder = provider.GetRequiredService<EnvironmentInfoProvider>();
                    var newContextRegistry = provider.GetRequiredService<Func<string, IMetricContextRegistry>>();

                    return new DefaultMetricsRegistry(loggerFactory, options, clock, envBuilder, newContextRegistry);
                });

            builder.Services.TryAddSingleton<IMetrics, DefaultMetrics>();
            builder.Services.TryAddSingleton(provider => new Lazy<IMetrics>(provider.GetRequiredService<IMetrics>));
            builder.Services.TryAddSingleton(provider => builder.Environment);
        }
    }
}