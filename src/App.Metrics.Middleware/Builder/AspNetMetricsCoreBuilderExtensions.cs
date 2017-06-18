// <copyright file="AspNetMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Middleware;
using App.Metrics.Middleware.DependencyInjection;
using App.Metrics.Middleware.DependencyInjection.Options;
using App.Metrics.Middleware.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AspNetMetricsCoreBuilderExtensions
        // ReSharper restore UnusedMember.Global
    {
        public static IMetricsHostBuilder AddMetricsMiddleware(
            this IMetricsHostBuilder builder,
            IConfiguration configuration,
            Action<IMetricsMiddlewareOptionsBuilder> setupMiddlewareOptionsAction)
        {
            builder.Services.Configure<AppMetricsMiddlewareOptions>(configuration);
            return builder.AddMetricsMiddlewareCore(setupMiddlewareOptionsAction);
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(
            this IMetricsHostBuilder builder,
            IConfiguration configuration,
            Action<AppMetricsMiddlewareOptions> setupAction,
            Action<IMetricsMiddlewareOptionsBuilder> setupMiddleware)
        {
            builder.Services.Configure<AppMetricsMiddlewareOptions>(configuration);
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddlewareCore(setupMiddleware);
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(
            this IMetricsHostBuilder builder,
            Action<AppMetricsMiddlewareOptions> setupOptionsAction,
            Action<IMetricsMiddlewareOptionsBuilder> setupMiddlewareOptionsAction)
        {
            builder.Services.Configure(setupOptionsAction);
            return builder.AddMetricsMiddlewareCore(setupMiddlewareOptionsAction);
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(
            this IMetricsHostBuilder builder,
            Action<IMetricsMiddlewareOptionsBuilder> setupMiddleware)
        {
            return builder.AddMetricsMiddlewareCore(setupMiddleware);
        }

        internal static IMetricsMiddlewareOptionsBuilder AddMetricsMiddlewareHostBuilder(this IMetricsHostBuilder metricsHostBuilder)
        {
            return new MetricsMiddlewareOptionsBuilder(metricsHostBuilder);
        }

        private static IMetricsHostBuilder AddMetricsMiddlewareCore(
            this IMetricsHostBuilder builder,
            Action<IMetricsMiddlewareOptionsBuilder> setupMiddleware)
        {
            setupMiddleware(builder.AddMetricsMiddlewareHostBuilder());

            builder.Services.TryAddSingleton<IEnvironmentInfoResponseWriter, NoOpEnvironmentInfoResponseWriter>();
            builder.Services.TryAddSingleton<IMetricsResponseWriter, NoOpMetricsResponseWriter>();
            builder.Services.TryAddSingleton<IMetricsTextResponseWriter, NoOpMetricsTextResponseWriter>();
            builder.Services.TryAddSingleton<IHealthResponseWriter, NoOpHealthStatusResponseWriter>();

            builder.AddRequiredAspNetPlatformServices();

            return builder;
        }
    }
}