// <copyright file="MetricsHostBuilderExtensionsCore.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Serialization;
using App.Metrics.Configuration;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Internal;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Filtering;
using App.Metrics.Health.Abstractions;
using App.Metrics.Health.Internal;
using App.Metrics.Infrastructure;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Registry.Internal;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Reporting.Internal;
using App.Metrics.ReservoirSampling;
using App.Metrics.Serialization;
using App.Metrics.Tagging;
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

            builder.Services.TryAddSingleton(provider => new DefaultSamplingReservoirProvider());
            builder.Services.TryAddSingleton<IReportFactory, NoOpReportFactory>();
            builder.Services.TryAddSingleton<IHealthCheckFactory, NoOpHealthCheckFactory>();
            builder.Services.TryAddSingleton<IClock, StopwatchClock>();
            builder.Services.TryAddSingleton<IFilterMetrics, DefaultMetricsFilter>();
            builder.Services.TryAddSingleton<EnvironmentInfoProvider, EnvironmentInfoProvider>();
            builder.Services.TryAddSingleton<IMetricDataSerializer, NoOpMetricDataSerializer>();
            builder.Services.TryAddSingleton<IHealthStatusSerializer, NoOpHealthStatusSerializer>();
            builder.Services.TryAddSingleton<IMeasureMetrics, DefaultMeasureMetricsProvider>();
            builder.Services.TryAddSingleton<IBuildMetrics, DefaultMetricsBuilderFactory>();
            builder.Services.TryAddSingleton<IProvideMetrics, DefaultMetricsProvider>();
            builder.Services.TryAddSingleton<IProvideMetricValues, DefaultMetricValuesProvider>();
            builder.Services.TryAddSingleton<IManageMetrics, DefaultMetricsManager>();
            builder.Services.TryAddSingleton<IProvideHealth, DefaultHealthProvider>();
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