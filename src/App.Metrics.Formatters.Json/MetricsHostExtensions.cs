// <copyright file="MetricsHostExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Internal;
using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Formatters.Json;
using App.Metrics.Formatters.Json.Abstractions.Serialization;
using App.Metrics.Formatters.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace

    public static class MetricsHostExtensions
    {
        /// <summary>
        ///     Enables JSON serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddJsonEnvironmentInfoSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, JsonEnvironmentInfoResponseWriter>());
            host.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoSerializer, EnvironmentInfoSerializer>());

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddJsonEnvironmentInfoSerialization(
            this IMetricsHostBuilder host,
            JsonSerializerSettings serializerSettings)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, JsonEnvironmentInfoResponseWriter>());
            host.Services.Replace(
                ServiceDescriptor.Transient<IEnvironmentInfoSerializer>(provider => new EnvironmentInfoSerializer(serializerSettings)));

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddJsonHealthSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, JsonHealthResponseWriter>());
            host.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer, HealthStatusSerializer>());

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IMetricsHostBuilder AddJsonHealthSerialization(this IMetricsHostBuilder host, JsonSerializerSettings serializerSettings)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, JsonHealthResponseWriter>());
            host.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer>(provider => new HealthStatusSerializer(serializerSettings)));

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddJsonMetricsSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, JsonMetricsResponseWriter>());
            host.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IMetricsHostBuilder AddJsonMetricsSerialization(this IMetricsHostBuilder host, JsonSerializerSettings serializerSettings)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, JsonMetricsResponseWriter>());
            host.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer>(provider => new MetricDataSerializer(serializerSettings)));

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric-text endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddJsonMetricsTextSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, JsonMetricsTextResponseWriter>());
            host.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric-text endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddJsonMetricsTextSerialization(this IMetricsHostBuilder host, JsonSerializerSettings serializerSettings)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, JsonMetricsTextResponseWriter>());
            host.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer>(provider => new MetricDataSerializer(serializerSettings)));

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric and health endpoint responses
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddJsonSerialization(this IMetricsHostBuilder host)
        {
            host.AddJsonHealthSerialization();
            host.AddJsonMetricsSerialization();
            host.AddJsonMetricsTextSerialization();

            return host;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric and health endpoint responses
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IMetricsHostBuilder AddJsonSerialization(this IMetricsHostBuilder host, JsonSerializerSettings serializerSettings)
        {
            host.AddJsonEnvironmentInfoSerialization(serializerSettings);
            host.AddJsonHealthSerialization(serializerSettings);
            host.AddJsonMetricsSerialization(serializerSettings);
            host.AddJsonMetricsTextSerialization(serializerSettings);

            return host;
        }
    }
}