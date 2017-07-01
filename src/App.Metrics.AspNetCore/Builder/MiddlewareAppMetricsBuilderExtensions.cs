// <copyright file="MiddlewareAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Middleware.Internal;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Core.DependencyInjection.Internal;
using App.Metrics.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class MiddlewareAppMetricsBuilderExtensions
        // ReSharper restore UnusedMember.Global
    {
        public static IAppMetricsBuilder AddMetricsMiddleware(
            this IAppMetricsBuilder builder,
            IConfiguration configuration,
            Action<IAppMetricsMiddlewareOptionsBuilder> setupMiddlewareOptionsAction)
        {
            builder.Services.Configure<AppMetricsMiddlewareOptions>(configuration);
            return builder.AddMetricsMiddlewareCore(setupMiddlewareOptionsAction);
        }

        public static IAppMetricsBuilder AddMetricsMiddleware(
            this IAppMetricsBuilder builder,
            IConfiguration configuration,
            Action<AppMetricsMiddlewareOptions> setupAction,
            Action<IAppMetricsMiddlewareOptionsBuilder> setupMiddleware)
        {
            builder.Services.Configure<AppMetricsMiddlewareOptions>(configuration);
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddlewareCore(setupMiddleware);
        }

        public static IAppMetricsBuilder AddMetricsMiddleware(
            this IAppMetricsBuilder builder,
            Action<AppMetricsMiddlewareOptions> setupOptionsAction,
            Action<IAppMetricsMiddlewareOptionsBuilder> setupMiddlewareOptionsAction)
        {
            builder.Services.Configure(setupOptionsAction);
            return builder.AddMetricsMiddlewareCore(setupMiddlewareOptionsAction);
        }

        public static IAppMetricsBuilder AddMetricsMiddleware(
            this IAppMetricsBuilder builder,
            Action<IAppMetricsMiddlewareOptionsBuilder> setupMiddleware)
        {
            return builder.AddMetricsMiddlewareCore(setupMiddleware);
        }

        internal static IAppMetricsMiddlewareOptionsBuilder AddAppMetricsMiddlewareBuilder(this IAppMetricsBuilder appMetricsBuilder)
        {
            return new AppMetricsMiddlewareOptionsBuilder(appMetricsBuilder);
        }

        private static IAppMetricsBuilder AddMetricsMiddlewareCore(
            this IAppMetricsBuilder builder,
            Action<IAppMetricsMiddlewareOptionsBuilder> setupMiddleware)
        {
            setupMiddleware(builder.AddAppMetricsMiddlewareBuilder());

            builder.Services.TryAddSingleton<IEnvironmentInfoResponseWriter, NoOpEnvironmentInfoResponseWriter>();
            builder.Services.TryAddSingleton<IMetricsResponseWriter, NoOpMetricsResponseWriter>();
            builder.Services.TryAddSingleton<IMetricsTextResponseWriter, NoOpMetricsTextResponseWriter>();

            builder.Services.TryAddSingleton<AppMetricsMiddlewareMarkerService, AppMetricsMiddlewareMarkerService>();
            builder.Services.AddRouting();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsMiddlewareOptions>>().Value);

            return builder;
        }
    }
}