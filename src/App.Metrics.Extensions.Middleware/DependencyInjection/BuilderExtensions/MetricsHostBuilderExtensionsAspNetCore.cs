// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Extensions.Middleware.DependencyInjection.Internal;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
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

        internal static void AddAspNetCoreServices(this IMetricsHostBuilder builder) { }
    }
}