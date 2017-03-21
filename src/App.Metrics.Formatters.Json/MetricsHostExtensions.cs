// <copyright file="MetricsHostExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.Serialization;
using App.Metrics.Core.Internal;
using App.Metrics.Formatters.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace

    public static class MetricsHostExtensions
    {
        public static IMetricsHostBuilder AddJsonSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());
            host.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer, HealthStatusSerializer>());

            return host;
        }

        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IMetricsHostBuilder AddJsonSerialization(this IMetricsHostBuilder host, JsonSerializerSettings serializerSettings)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer>(provider => new MetricDataSerializer(serializerSettings)));
            host.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer>(provider => new HealthStatusSerializer(serializerSettings)));

            return host;
        }
    }
}