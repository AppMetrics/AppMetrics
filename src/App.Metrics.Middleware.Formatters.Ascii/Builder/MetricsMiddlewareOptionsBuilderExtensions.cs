// <copyright file="MetricsMiddlewareOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Middleware;
using App.Metrics.Middleware.DependencyInjection;
using App.Metrics.Middleware.Formatters.Ascii;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class MetricsMiddlewareOptionsBuilderExtensions
    {
        /// <summary>
        ///     Enables Plain Text serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddAsciiEnvironmentInfoSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, AsciiEnvironmentInfoResponseWriter>());

            return options;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the health endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddAsciiHealthSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, AsciiHealthResponseWriter>());

            return options;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddAsciiMetricsSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, AsciiMetricsResponseWriter>());

            return options;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddAsciiMetricsTextSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.MetricsHostBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, AsciiMetricsTextResponseWriter>());

            return options;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the metric and health endpoint responses
        /// </summary>
        /// <param name="options">The metrics middleware options builder.</param>
        /// <returns>The metrics middleware options builder</returns>
        public static IMetricsMiddlewareOptionsBuilder AddAsciiSerialization(this IMetricsMiddlewareOptionsBuilder options)
        {
            options.AddAsciiEnvironmentInfoSerialization();
            options.AddAsciiHealthSerialization();
            options.AddAsciiMetricsSerialization();
            options.AddAsciiMetricsTextSerialization();

            return options;
        }
    }
}