// <copyright file="MetricsAppMetricsHealthChecksBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Builder;
using App.Metrics.HealthMetrics.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsAppMetricsHealthChecksBuilderExtensions
    {
        public static IAppMetricsHealthChecksBuilder AddHealthCheckMetrics(this IAppMetricsHealthChecksBuilder builder)
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IProvideHealth, HealthWithMetricsProvider>());

            return builder;
        }
    }
}
