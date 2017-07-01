// <copyright file="AppMetricsMiddlewareOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.AspNetCore.Formatters.Json;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsMiddlewareOptionsBuilderExtensions
    {
        /// <summary>
        ///     Enables JSON serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddEnvironmentJsonFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, JsonEnvironmentInfoResponseWriter>());
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoSerializer, EnvironmentInfoSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddEnvironmentJsonFormatters(
            this IAppMetricsMiddlewareOptionsBuilder options,
            JsonSerializerSettings serializerSettings)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, JsonEnvironmentInfoResponseWriter>());
            options.AppMetricsBuilder.Services.Replace(
                ServiceDescriptor.Transient<IEnvironmentInfoSerializer>(provider => new EnvironmentInfoSerializer(serializerSettings)));

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsJsonFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, JsonMetricsResponseWriter>());
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsJsonFormatters(this IAppMetricsMiddlewareOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, JsonMetricsResponseWriter>());
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer>(provider => new MetricDataSerializer(serializerSettings)));

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric-text endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsTextJsonFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, JsonMetricsTextResponseWriter>());
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metric-text endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsTextJsonFormatters(this IAppMetricsMiddlewareOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, JsonMetricsTextResponseWriter>());
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer>(provider => new MetricDataSerializer(serializerSettings)));

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metrics endpoints responses
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddJsonFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AddMetricsJsonFormatters();
            options.AddMetricsTextJsonFormatters();

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the metrics endpoints responses
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IAppMetricsMiddlewareOptionsBuilder AddJsonFormatters(this IAppMetricsMiddlewareOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.AddEnvironmentJsonFormatters(serializerSettings);
            options.AddMetricsJsonFormatters(serializerSettings);
            options.AddMetricsTextJsonFormatters(serializerSettings);

            return options;
        }
    }
}