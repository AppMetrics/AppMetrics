// <copyright file="AspNetMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Extensions.Middleware.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace
    public static class AspNetMetricsCoreBuilderExtensions
        // ReSharper restore UnusedMember.Global
    {
        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<AspNetMetricsOptions>(configuration);
            return builder.AddMetricsMiddlewareCore();
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(
            this IMetricsHostBuilder builder,
            IConfiguration configuration,
            Action<AspNetMetricsOptions> setupAction)
        {
            builder.Services.Configure<AspNetMetricsOptions>(configuration);
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddlewareCore();
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder, Action<AspNetMetricsOptions> setupAction)
        {
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddlewareCore();
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder)
        {
            return builder.AddMetricsMiddlewareCore();
        }

        private static IMetricsHostBuilder AddMetricsMiddlewareCore(this IMetricsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<IEnvironmentInfoResponseWriter, NoOpEnvironmentInfoResponseWriter>();
            builder.Services.TryAddSingleton<IMetricsResponseWriter, NoOpMetricsResponseWriter>();
            builder.Services.TryAddSingleton<IMetricsTextResponseWriter, NoOpMetricsTextResponseWriter>();
            builder.Services.TryAddSingleton<IHealthResponseWriter, NoOpHealthStatusResponseWriter>();

            builder.AddRequiredAspNetPlatformServices();

            return builder;
        }
    }
}