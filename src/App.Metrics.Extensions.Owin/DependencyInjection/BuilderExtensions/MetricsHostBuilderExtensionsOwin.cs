// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Extensions.Owin.DependencyInjection.Internal;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsHostBuilderExtensionsOwin
    {
        public static IMetricsHostBuilder AddRequiredAspNetPlatformServices(this IMetricsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<OwinMetricsMarkerService, OwinMetricsMarkerService>();
            builder.Services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<OwinMetricsOptions>>().Value);

            return builder;
        }
    }
}