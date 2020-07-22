// <copyright file="MetricsAspNetTrackingWebHostBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Tracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
// ReSharper restore CheckNamespace
{
    public static class MetricsAspNetTrackingHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" /> cannot be null
        /// </exception>
        public static IHostBuilder UseMetricsWebTracking(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsTrackingMiddleware(context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" /> cannot be null
        /// </exception>
        public static IHostBuilder UseMetricsWebTracking(
            this IHostBuilder hostBuilder,
            Action<MetricsWebTrackingOptions> optionsDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsTrackingMiddleware(optionsDelegate, context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.</param>
        /// <param name="setupDelegate">A callback to configure <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" /> cannot be null
        /// </exception>
        public static IHostBuilder UseMetricsWebTracking(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, MetricsWebTrackingOptions> setupDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var trackingMiddlewareOptions = new MetricsWebTrackingOptions();
                    services.AddMetricsTrackingMiddleware(
                        options => setupDelegate(context, trackingMiddlewareOptions),
                        context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing <see cref="MetricsWebTrackingOptions"/></param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" /> cannot be null
        /// </exception>
        public static IHostBuilder UseMetricsWebTracking(
            this IHostBuilder hostBuilder,
            IConfiguration configuration,
            Action<MetricsWebTrackingOptions> optionsDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsTrackingMiddleware(optionsDelegate, configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter());
                });

            return hostBuilder;
        }
    }
}