// <copyright file="AppMetricsMiddlewareHealthChecksOptionsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Health.Formatters.Ascii;
using App.Metrics.Health;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsMiddlewareHealthChecksOptionsBuilderExtensions
    {
        /// <summary>
        ///     Enables Plain Text serialization on the health endpoint's response
        /// </summary>
        /// <param name="options">The metrics middleware options checksBuilder.</param>
        /// <returns>The metrics middleware options checksBuilder</returns>
        public static IAppMetricsMiddlewareHealthChecksOptionsBuilder AddAsciiFormatters(this IAppMetricsMiddlewareHealthChecksOptionsBuilder options)
        {
            options.AppMetricsHealthChecksChecksBuilder.Services.Replace(ServiceDescriptor.Transient<IHealthResponseWriter, AsciiHealthResponseWriter>());

            return options;
        }
    }
}