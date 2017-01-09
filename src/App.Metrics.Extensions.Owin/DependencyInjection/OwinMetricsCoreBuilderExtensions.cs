// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class OwinMetricsCoreBuilderExtensions
    {
        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder)
        {
            builder.AddRequiredAspNetPlatformServices();
            return builder;
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<OwinMetricsOptions>(configuration);
            return builder.AddMetricsMiddleware();
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder, IConfiguration configuration,
            Action<OwinMetricsOptions> setupAction)
        {
            builder.Services.Configure<OwinMetricsOptions>(configuration);
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddleware();
        }

        public static IMetricsHostBuilder AddMetricsMiddleware(this IMetricsHostBuilder builder, Action<OwinMetricsOptions> setupAction)
        {
            builder.Services.Configure(setupAction);
            return builder.AddMetricsMiddleware();
        }
    }
}