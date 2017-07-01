// <copyright file="AppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.DependencyInjection.Internal;
using App.Metrics.Core.Filtering;
using App.Metrics.Core.Infrastructure;
using App.Metrics.Core.Internal;
using App.Metrics.Core.Internal.NoOp;
using App.Metrics.Core.ReservoirSampling;
using App.Metrics.Core.Tagging;
using App.Metrics.Filters;
using App.Metrics.Registry;
using App.Metrics.Reporting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsBuilderExtensions
    {
        internal static IAppMetricsBuilder AddRequiredPlatformServices(this IAppMetricsBuilder builder)
        {
            builder.Services.TryAddSingleton<AppMetricsMarkerService, AppMetricsMarkerService>();
            builder.Services.AddOptions();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsOptions>>().Value);
            builder.Services.AddSingleton<IConfigureOptions<AppMetricsOptions>, ConfigureAppMetricsOptions>();

            return builder;
        }

        internal static void AddCoreServices(this IAppMetricsBuilder builder)
        {
            builder.Services.TryAddTransient<Func<string, IMetricContextRegistry>>(
                provider =>
                {
                    var globalTags = provider.GetRequiredService<AppMetricsOptions>().GlobalTags;
                    return context => new DefaultMetricContextRegistry(context, new GlobalMetricTags(globalTags));
                });

            builder.Services.TryAddSingleton(provider => new DefaultSamplingReservoirProvider());
            builder.Services.TryAddSingleton<IReportFactory, NoOpReportFactory>();
            builder.Services.TryAddSingleton<IClock, StopwatchClock>();
            builder.Services.TryAddSingleton<IFilterMetrics, DefaultMetricsFilter>();
            builder.Services.TryAddSingleton<EnvironmentInfoProvider, EnvironmentInfoProvider>();
            builder.Services.TryAddSingleton<IMeasureMetrics, DefaultMeasureMetricsProvider>();
            builder.Services.TryAddSingleton<IBuildMetrics, DefaultMetricsBuilderFactory>();
            builder.Services.TryAddSingleton<IProvideMetrics, DefaultMetricsProvider>();
            builder.Services.TryAddSingleton<IProvideMetricValues, DefaultMetricValuesProvider>();
            builder.Services.TryAddSingleton<IManageMetrics, DefaultMetricsManager>();
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
                    var newContextRegistry = provider.GetRequiredService<Func<string, IMetricContextRegistry>>();

                    return new DefaultMetricsRegistry(loggerFactory, options, clock, newContextRegistry);
                });

            builder.Services.TryAddSingleton<IMetrics, DefaultMetrics>();
            builder.Services.TryAddSingleton(provider => new Lazy<IMetrics>(provider.GetRequiredService<IMetrics>));
            builder.Services.TryAddSingleton(provider => builder.Environment);
        }
    }
}