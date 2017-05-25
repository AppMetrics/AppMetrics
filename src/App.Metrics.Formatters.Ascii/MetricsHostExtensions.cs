// <copyright file="MetricsHostExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Extensions.Middleware.Abstractions;
using App.Metrics.Formatters.Ascii;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsHostExtensions
    {
        /// <summary>
        /// Enables Plain Text serialization on the health endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddAsciiHealthSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, AsciiHealthResponseWriter>());

            return host;
        }

        /// <summary>
        /// Enables Plain Text serialization on the metric endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddAsciiMetricsSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, AsciiMetricsResponseWriter>());

            return host;
        }

        /// <summary>
        /// Enables Plain Text serialization on the metric endpoint's response
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddAsciiMetricsTextSerialization(this IMetricsHostBuilder host)
        {
            host.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, AsciiMetricsTextResponseWriter>());

            return host;
        }

        /// <summary>
        /// Enables Plain Text serialization on the metric and health endpoint responses
        /// </summary>
        /// <param name="host">The metrics host builder.</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddAsciiSerialization(this IMetricsHostBuilder host)
        {
            host.AddAsciiHealthSerialization();
            host.AddAsciiMetricsSerialization();
            host.AddAsciiMetricsTextSerialization();

            return host;
        }
    }
}