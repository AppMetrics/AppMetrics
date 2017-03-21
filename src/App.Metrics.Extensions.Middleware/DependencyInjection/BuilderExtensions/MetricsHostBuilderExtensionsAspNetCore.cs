// <copyright file="MetricsHostBuilderExtensionsAspNetCore.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Extensions.Middleware.DependencyInjection.Internal;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMethodReturnValue.Global
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace
    public static class MetricsHostBuilderExtensionsAspNetCore
    {
        public static IMetricsHostBuilder AddRequiredAspNetPlatformServices(this IMetricsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<AspNetMetricsMarkerService, AspNetMetricsMarkerService>();
            builder.Services.AddRouting();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AspNetMetricsOptions>>().Value);

            return builder;
        }
    }

    // ReSharper restore UnusedMethodReturnValue.Global
}