// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace
    public static class AspNetMetricsCoreBuilderExtensions
    {
        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder)
        {
            builder.AddRequiredAspNetPlatformServices();
            builder.AddAspNetCoreServices();

            return builder;
        }

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
    }
}