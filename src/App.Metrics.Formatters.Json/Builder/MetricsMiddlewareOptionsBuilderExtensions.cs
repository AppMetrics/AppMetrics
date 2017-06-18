// <copyright file="MetricsMiddlewareOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.Formatters.Json;
using App.Metrics.Formatters.Json.Abstractions.Serialization;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Middleware;
using App.Metrics.Middleware.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
// ReSharper restore CheckNamespace
{
    public static class MetricsMiddlewareOptionsBuilderExtensions
    {
        /// <summary>
        ///     Enables JSON serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddJsonEnvironmentInfoSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, JsonEnvironmentInfoResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoSerializer, EnvironmentInfoSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddJsonEnvironmentInfoSerialization(
            this IMetricsMiddlewareOptionsBuilder options,
            JsonSerializerSettings serializerSettings)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, JsonEnvironmentInfoResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(
                ServiceDescriptor.Transient<IEnvironmentInfoSerializer>(provider => new EnvironmentInfoSerializer(serializerSettings)));

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddJsonHealthSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, JsonHealthResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer, HealthStatusSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options builder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IMetricsMiddlewareOptionsBuilder AddJsonHealthSerialization(this IMetricsMiddlewareOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, JsonHealthResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer>(provider => new HealthStatusSerializer(serializerSettings)));

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddJsonMetricsSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, JsonMetricsResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options builder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IMetricsMiddlewareOptionsBuilder AddJsonMetricsSerialization(this IMetricsMiddlewareOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, JsonMetricsResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer>(provider => new MetricDataSerializer(serializerSettings)));

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric-text endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddJsonMetricsTextSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, JsonMetricsTextResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric-text endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddJsonMetricsTextSerialization(this IMetricsMiddlewareOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, JsonMetricsTextResponseWriter>());
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer>(provider => new MetricDataSerializer(serializerSettings)));

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric and health endpoint responses
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddJsonSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.AddJsonHealthSerialization();
            options.AddJsonMetricsSerialization();
            options.AddJsonMetricsTextSerialization();

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric and health endpoint responses
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options builder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IMetricsMiddlewareOptionsBuilder AddJsonSerialization(this IMetricsMiddlewareOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.AddJsonEnvironmentInfoSerialization(serializerSettings);
            options.AddJsonHealthSerialization(serializerSettings);
            options.AddJsonMetricsSerialization(serializerSettings);
            options.AddJsonMetricsTextSerialization(serializerSettings);

            return options;
        }
    }
}