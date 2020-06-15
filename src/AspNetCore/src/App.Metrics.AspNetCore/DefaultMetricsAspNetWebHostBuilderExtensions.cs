// <copyright file="DefaultMetricsAspNetWebHostBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Tracking;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore
{
    public static class DefaultMetricsAspNetWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetrics(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsReportingHostedService();
                    services.AddMetricsEndpoints(context.Configuration);
                    services.AddMetricsTrackingMiddleware(context.Configuration);
                });

            hostBuilder.UseMetricsEndpoints();
            hostBuilder.UseMetricsWebTracking();

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetrics(
            this IWebHostBuilder hostBuilder,
            Action<MetricsWebHostOptions> optionsDelegate)
        {
            var options = new MetricsWebHostOptions();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    optionsDelegate(options);

                    services.AddMetricsReportingHostedService(options.UnobservedTaskExceptionHandler);
                    services.AddMetricsEndpoints(options.EndpointOptions);
                    services.AddMetricsTrackingMiddleware(options.TrackingMiddlewareOptions);
                });

            hostBuilder.UseMetricsEndpoints();
            hostBuilder.UseMetricsWebTracking();

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetrics(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, MetricsWebHostOptions> optionsDelegate)
        {
            var options = new MetricsWebHostOptions();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    optionsDelegate(context, options);

                    services.AddMetricsReportingHostedService(options.UnobservedTaskExceptionHandler);
                    services.AddMetricsEndpoints(options.EndpointOptions, context.Configuration);
                    services.AddMetricsTrackingMiddleware(options.TrackingMiddlewareOptions, context.Configuration);
                });

            hostBuilder.UseMetricsEndpoints();
            hostBuilder.UseMetricsWebTracking();

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <typeparam name="TStartup">The type of the <see cref="IStartupFilter" /> used to configure metrics middleware.</typeparam>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IWebHostBuilder UseMetrics<TStartup>(
            this IWebHostBuilder hostBuilder,
            Action<MetricsWebHostOptions> optionsDelegate)
            where TStartup : IStartupFilter, new()
        {
            var options = new MetricsWebHostOptions();

            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    optionsDelegate(options);

                    services.AddMetricsReportingHostedService(options.UnobservedTaskExceptionHandler);
                    services.AddMetricsEndpoints(options.EndpointOptions, context.Configuration);
                    services.AddMetricsTrackingMiddleware(options.TrackingMiddlewareOptions, context.Configuration);
                    services.AddSingleton<IStartupFilter>(new TStartup());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <typeparam name="TStartup">The type of the <see cref="IStartupFilter" /> used to configure metrics middleware.</typeparam>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IWebHostBuilder UseMetrics<TStartup>(
            this IWebHostBuilder hostBuilder)
            where TStartup : IStartupFilter, new()
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsReportingHostedService();
                    services.AddMetricsEndpoints(context.Configuration);
                    services.AddMetricsTrackingMiddleware(context.Configuration);
                    services.AddSingleton<IStartupFilter>(new TStartup());
                });

            return hostBuilder;
        }
    }
}