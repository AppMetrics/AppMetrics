// <copyright file="MetricsHostBuilderExtensionsAspNetCore.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Middleware.Internal;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsHostBuilderExtensionsAspNetCore
    {
        public static IMetricsHostBuilder AddRequiredAspNetPlatformServices(this IMetricsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<AspNetMetricsMarkerService, AspNetMetricsMarkerService>();
            builder.Services.AddRouting();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsMiddlewareOptions>>().Value);

            return builder;
        }
    }

    // ReSharper restore UnusedMethodReturnValue.Global
}