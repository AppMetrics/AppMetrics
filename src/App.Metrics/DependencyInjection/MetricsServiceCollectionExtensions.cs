// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsServiceCollectionExtensions
    {
        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services)
        {
            var builder = services.AddMetricsHostBuilder();

            builder.AddRequiredPlatformServices();

            builder.AddCoreServices();

            return new MetricsHostBuilder(services);
        }

        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppMetricsOptions>(configuration);
            return services.AddMetrics();
        }

        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services, Action<AppMetricsOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddMetrics();
        }

        internal static IMetricsHostBuilder AddMetricsHostBuilder(this IServiceCollection services)
        {
            return new MetricsHostBuilder(services);
        }
    }
}