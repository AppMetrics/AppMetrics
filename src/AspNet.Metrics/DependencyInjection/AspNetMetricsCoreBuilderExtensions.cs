// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using AspNet.Metrics.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class AspNetMetricsCoreBuilderExtensions
    {
        public static IMetricsHostBuilder AddAspNetMetrics(this IMetricsHostBuilder builder)
        {
            builder.AddRequiredAspNetPlatformServices();
            builder.AddAspNetCoreServices();

            return builder;
        }

        public static IMetricsHostBuilder AddAspNetMetrics(this IMetricsHostBuilder builder, IConfiguration configuration)
        {
            builder.Services.Configure<AspNetMetricsOptions>(configuration);
            return builder.AddAspNetMetrics();
        }

        public static IMetricsHostBuilder AddAspNetMetrics(this IMetricsHostBuilder builder, Action<AspNetMetricsOptions> setupAction)
        {
            builder.Services.Configure(setupAction);
            return builder.AddAspNetMetrics();
        }
    }
}