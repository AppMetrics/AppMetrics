// <copyright file="AppMetricsMiddlewareOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Formatters.Ascii;
using App.Metrics.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsMiddlewareOptionsBuilderExtensions
    {
        /// <summary>
        ///     Enables Plain Text serialization on the environment info endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddEnvironmentAsciiFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IEnvironmentInfoResponseWriter, AsciiEnvironmentInfoResponseWriter>());

            return options;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsAsciiFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsResponseWriter, AsciiMetricsResponseWriter>());

            return options;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the metric endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddMetricsTextAsciiFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AppMetricsBuilder.Services.Replace(ServiceDescriptor.Transient<IMetricsTextResponseWriter, AsciiMetricsTextResponseWriter>());

            return options;
        }

        /// <summary>
        ///     Enables Plain Text serialization on the metric endpoint responses
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareOptionsBuilder AddAsciiFormatters(this IAppMetricsMiddlewareOptionsBuilder options)
        {
            options.AddEnvironmentAsciiFormatters();
            options.AddMetricsAsciiFormatters();
            options.AddMetricsTextAsciiFormatters();

            return options;
        }
    }
}