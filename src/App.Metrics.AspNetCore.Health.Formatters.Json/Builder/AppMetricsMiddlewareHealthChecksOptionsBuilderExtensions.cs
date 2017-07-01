// <copyright file="AppMetricsMiddlewareHealthChecksOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.AspNetCore.Health.Formatters.Json;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsMiddlewareHealthChecksOptionsBuilderExtensions
    {
        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareHealthChecksOptionsBuilder AddJsonFormatters(this IAppMetricsMiddlewareHealthChecksOptionsBuilder options)
        {
            options.AppMetricsHealthChecksChecksBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, JsonHealthResponseWriter>());
            options.AppMetricsHealthChecksChecksBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer, HealthStatusSerializer>());

            return options;
        }

        /// <summary>
        ///     Enables JSON serialization on the health endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <param name="serializerSettings">The JSON serializer settings.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test JsonSerializerSettings really
        public static IAppMetricsMiddlewareHealthChecksOptionsBuilder AddJsonFormatters(this IAppMetricsMiddlewareHealthChecksOptionsBuilder options, JsonSerializerSettings serializerSettings)
        {
            options.AppMetricsHealthChecksChecksBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, JsonHealthResponseWriter>());
            options.AppMetricsHealthChecksChecksBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer>(provider => new HealthStatusSerializer(serializerSettings)));

            return options;
        }
    }
}