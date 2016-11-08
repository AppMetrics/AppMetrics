// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsServiceCollectionExtensions
    {
        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddMetrics(options => { }, default(IMetricsContext));
        }

        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services,
            Action<AppMetricsOptions> setupAction, IMetricsContext metricsContext)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = services.AddMetricsCore(setupAction, metricsContext);

            return builder;
        }

        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services, Action<AppMetricsOptions> setupAction)
        {
            return services.AddMetrics(setupAction, default(IMetricsContext));
        }
    }
}