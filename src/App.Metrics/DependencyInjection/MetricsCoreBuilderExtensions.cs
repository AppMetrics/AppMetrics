// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using App.Metrics.DependencyInjection;
using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    internal static class MetricsCoreBuilderExtensions
    {
        internal static IServiceCollection AddHealthChecks(this IServiceCollection services, IMetricsEnvironment environment)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            HealthChecksAsServices.AddHealthChecksAsServices(services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(environment.ApplicationName));

            return services;
        }
    }
}