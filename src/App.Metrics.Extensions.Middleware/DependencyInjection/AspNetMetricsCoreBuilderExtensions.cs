// <copyright file="AspNetMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;

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
            return builder.AddMetricsMiddleware();
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(
            this IMetricsHostBuilder builder,
            IConfiguration configuration,
            Action<AspNetMetricsOptions> setupAction)
        {
            builder.Services.Configure<AspNetMetricsOptions>(configuration);
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddleware();
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder, Action<AspNetMetricsOptions> setupAction)
        {
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddleware();
        }

        private static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder)
        {
            builder.AddRequiredAspNetPlatformServices();

            return builder;
        }
    }
}