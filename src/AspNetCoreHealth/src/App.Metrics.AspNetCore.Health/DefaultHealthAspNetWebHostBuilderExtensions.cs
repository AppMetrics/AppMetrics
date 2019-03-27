// <copyright file="DefaultHealthAspNetWebHostBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Health
{
    public static class DefaultHealthAspNetWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealth(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealthReportingHostedService();
                    services.AddHealthEndpoints(context.Configuration);
                });

            hostBuilder.UseHealthEndpoints();

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="HealthWebHostOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealth(
            this IWebHostBuilder hostBuilder,
            Action<HealthWebHostOptions> optionsDelegate)
        {
            var options = new HealthWebHostOptions();

            hostBuilder.ConfigureServices(
                services =>
                {
                    optionsDelegate(options);

                    services.AddHealthReportingHostedService(options.UnobservedTaskExceptionHandler);
                    services.AddHealthEndpoints(options.EndpointOptions);
                });

            hostBuilder.UseHealthEndpoints();

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="HealthWebHostOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealth(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, HealthWebHostOptions> optionsDelegate)
        {
            var options = new HealthWebHostOptions();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    optionsDelegate(context, options);

                    services.AddHealthReportingHostedService(options.UnobservedTaskExceptionHandler);
                    services.AddHealthEndpoints(options.EndpointOptions, context.Configuration);
                });

            hostBuilder.UseHealthEndpoints();

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <typeparam name="TStartup">The type of the <see cref="IStartupFilter" /> used to configure metrics middleware.</typeparam>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="HealthWebHostOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IWebHostBuilder UseHealth<TStartup>(
            this IWebHostBuilder hostBuilder,
            Action<HealthWebHostOptions> optionsDelegate)
            where TStartup : IStartupFilter, new()
        {
            var options = new HealthWebHostOptions();

            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    optionsDelegate(options);

                    services.AddHealthReportingHostedService(options.UnobservedTaskExceptionHandler);
                    services.AddHealthEndpoints(options.EndpointOptions, context.Configuration);
                    services.AddSingleton<IStartupFilter>(new TStartup());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <typeparam name="TStartup">The type of the <see cref="IStartupFilter" /> used to configure metrics middleware.</typeparam>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IWebHostBuilder UseHealth<TStartup>(
            this IWebHostBuilder hostBuilder)
            where TStartup : IStartupFilter, new()
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealthReportingHostedService();
                    services.AddHealthEndpoints(context.Configuration);
                    services.AddSingleton<IStartupFilter>(new TStartup());
                });

            return hostBuilder;
        }
    }
}