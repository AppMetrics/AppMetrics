// <copyright file="ServiceCollectionMetricsBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Extensions.DependencyInjection
{
    public static class ServiceCollectionMetricsBuilderExtensions
    {
        public static IMetricsRoot BuildAndAddTo(
            this IMetricsBuilder builder,
            IServiceCollection services)
        {
            var metrics = builder.Build();

            services.AddMetrics(metrics);

            return metrics;
        }
    }
}
