// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Metrics.Formatters.Json
{
    public static class MetricsHostExtensions
    {
        public static IMetricsHost AddJsonSerialization(this IMetricsHost host)
        {
            AddJsonNetCoreServices(host.Services);

            return host;
        }

        internal static void AddJsonNetCoreServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());
            services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer, HealthStatusSerializer>());
        }
    }
}