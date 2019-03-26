// <copyright file="HealthAspNetEndpointWebHostBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.AspNetCore.Health.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class HealthAspNetEndpointWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Helath services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealthEndpoints(context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="HealthEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(
            this IWebHostBuilder hostBuilder,
            Action<HealthEndpointsOptions> optionsDelegate)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealthEndpoints(optionsDelegate, context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupDelegate">A callback to configure <see cref="HealthEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, HealthEndpointsOptions> setupDelegate)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var endpointOptions = new HealthEndpointsOptions();
                    services.AddHealthEndpoints(
                        options => setupDelegate(context, endpointOptions),
                        context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Health services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> containing <see cref="HealthEndpointsOptions" /></param>
        /// <param name="optionsDelegate">A callback to configure <see cref="HealthEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseHealthEndpoints(
            this IWebHostBuilder hostBuilder,
            IConfiguration configuration,
            Action<HealthEndpointsOptions> optionsDelegate)
        {
            hostBuilder.ConfigureHealth();

            hostBuilder.ConfigureServices(
                services =>
                {
                    services.AddHealthEndpoints(optionsDelegate, configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureAppHealthHostingConfiguration(
            this IWebHostBuilder hostBuilder,
            Action<HealthEndpointsHostingOptions> setupHostingConfiguration)
        {
            var healthEndpointHostingOptions = new HealthEndpointsHostingOptions();
            setupHostingConfiguration(healthEndpointHostingOptions);

            var ports = new List<int>();

            if (healthEndpointHostingOptions.AllEndpointsPort.HasValue)
            {
                Console.WriteLine(
                    $"Hosting {healthEndpointHostingOptions.HealthEndpoint} endpoint on port {healthEndpointHostingOptions.AllEndpointsPort.Value}");
                Console.WriteLine(
                    $"Hosting {healthEndpointHostingOptions.PingEndpoint} on port {healthEndpointHostingOptions.AllEndpointsPort.Value}");

                ports.Add(healthEndpointHostingOptions.AllEndpointsPort.Value);
            }
            else
            {
                if (healthEndpointHostingOptions.HealthEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {healthEndpointHostingOptions.HealthEndpoint} on port {healthEndpointHostingOptions.HealthEndpointPort.Value}");
                    ports.Add(healthEndpointHostingOptions.HealthEndpointPort.Value);
                }

                if (healthEndpointHostingOptions.PingEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {healthEndpointHostingOptions.PingEndpoint} on port {healthEndpointHostingOptions.PingEndpointPort.Value}");
                    ports.Add(healthEndpointHostingOptions.PingEndpointPort.Value);
                }
            }

            if (ports.Any())
            {
                var existingUrl = hostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
                var additionalUrls = string.Join(";", ports.Distinct().Select(p => $"http://*:{p}/"));
                hostBuilder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};{additionalUrls}");
            }

            hostBuilder.ConfigureServices(services => services.Configure(setupHostingConfiguration));

            return hostBuilder;
        }
    }
}