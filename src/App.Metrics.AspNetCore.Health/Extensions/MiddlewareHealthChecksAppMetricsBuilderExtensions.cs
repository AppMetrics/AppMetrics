// <copyright file="MiddlewareHealthChecksAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Internal;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class MiddlewareHealthChecksAppMetricsBuilderExtensions
    {
        public static IAppMetricsHealthChecksBuilder AddHealthCheckMiddleware(
            this IAppMetricsHealthChecksBuilder checksBuilder,
            IConfiguration configuration,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddlewareOptionsAction)
        {
            checksBuilder.Services.Configure<AppMetricsMiddlewareHealthChecksOptions>(configuration);
            return checksBuilder.AddMetricsMiddlewareHealthChecksCore(setupMiddlewareOptionsAction);
        }

        public static IAppMetricsHealthChecksBuilder AddHealthCheckMiddleware(
            this IAppMetricsHealthChecksBuilder checksBuilder,
            IConfiguration configuration,
            Action<AppMetricsMiddlewareHealthChecksOptions> setupAction,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddleware)
        {
            checksBuilder.Services.Configure<AppMetricsMiddlewareHealthChecksOptions>(configuration);
            checksBuilder.Services.Configure(setupAction);
            return checksBuilder.AddMetricsMiddlewareHealthChecksCore(setupMiddleware);
        }

        public static IAppMetricsHealthChecksBuilder AddHealthCheckMiddleware(
            this IAppMetricsHealthChecksBuilder checksBuilder,
            Action<AppMetricsMiddlewareHealthChecksOptions> setupOptionsAction,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddlewareOptionsAction)
        {
            checksBuilder.Services.Configure(setupOptionsAction);
            return checksBuilder.AddMetricsMiddlewareHealthChecksCore(setupMiddlewareOptionsAction);
        }

        public static IAppMetricsHealthChecksBuilder AddHealthCheckMiddleware(
            this IAppMetricsHealthChecksBuilder checksBuilder,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddleware)
        {
            return checksBuilder.AddMetricsMiddlewareHealthChecksCore(setupMiddleware);
        }

        internal static IAppMetricsMiddlewareHealthChecksOptionsBuilder AddAppMetricsMiddlewareHealthChecksBuilder(this IAppMetricsHealthChecksBuilder appMetricsHealthCheckBuilder)
        {
            return new AppMetricsMiddlewareHealthChecksOptionsBuilder(appMetricsHealthCheckBuilder);
        }

        private static IAppMetricsHealthChecksBuilder AddMetricsMiddlewareHealthChecksCore(
            this IAppMetricsHealthChecksBuilder checksBuilder,
            Action<IAppMetricsMiddlewareHealthChecksOptionsBuilder> setupMiddleware)
        {
            setupMiddleware(checksBuilder.AddAppMetricsMiddlewareHealthChecksBuilder());

            checksBuilder.Services.TryAddSingleton<IHealthResponseWriter, NoOpHealthStatusResponseWriter>();

            checksBuilder.Services.TryAddSingleton<AppMetricsMiddlewareHealthChecksMarkerService, AppMetricsMiddlewareHealthChecksMarkerService>();
            checksBuilder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsMiddlewareHealthChecksOptions>>().Value);

            return checksBuilder;
        }
    }
}