// <copyright file="MetricsAspNetEndpointWebHostBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.AspNetCore.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
// ReSharper restore CheckNamespace
{
    public static class MetricsAspNetEndpointHostBuilderExtensions
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
        public static IHostBuilder UseMetricsEndpoints(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsEndpoints(context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.</param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" /> cannot be null
        /// </exception>
        public static IHostBuilder UseMetricsEndpoints(
            this IHostBuilder hostBuilder,
            Action<MetricEndpointsOptions> optionsDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsEndpoints(optionsDelegate, context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.</param>
        /// <param name="setupDelegate">A callback to configure <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" /> cannot be null
        /// </exception>
        public static IHostBuilder UseMetricsEndpoints(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, MetricEndpointsOptions> setupDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var endpointOptions = new MetricEndpointsOptions();
                    services.AddMetricsEndpoints(
                        options => setupDelegate(context, endpointOptions),
                        context.Configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics services, configuration and middleware to the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> containing <see cref="MetricEndpointsOptions" /></param>
        /// <param name="optionsDelegate">A callback to configure <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IHostBuilder" /> cannot be null
        /// </exception>
        public static IHostBuilder UseMetricsEndpoints(
            this IHostBuilder hostBuilder,
            IConfiguration configuration,
            Action<MetricEndpointsOptions> optionsDelegate)
        {
            hostBuilder.ConfigureMetrics();

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsEndpoints(optionsDelegate, configuration);
                    services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
                });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureAppMetricsHostingConfiguration(
            this IHostBuilder hostBuilder,
            Action<MetricsEndpointsHostingOptions> setupHostingConfiguration)
        {
            var metricsEndpointHostingOptions = new MetricsEndpointsHostingOptions();
            setupHostingConfiguration(metricsEndpointHostingOptions);

            var ports = new List<int>();

            if (metricsEndpointHostingOptions.AllEndpointsPort.HasValue)
            {
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsEndpoint} on port {metricsEndpointHostingOptions.AllEndpointsPort.Value}");
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsTextEndpoint} endpoint on port {metricsEndpointHostingOptions.AllEndpointsPort.Value}");
                Console.WriteLine($"Hosting {metricsEndpointHostingOptions.EnvironmentInfoEndpoint} endpoint on port {metricsEndpointHostingOptions.AllEndpointsPort.Value}");

                ports.Add(metricsEndpointHostingOptions.AllEndpointsPort.Value);
            }
            else
            {
                if (metricsEndpointHostingOptions.MetricsEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsEndpoint} on port {metricsEndpointHostingOptions.MetricsEndpointPort.Value}");
                    ports.Add(metricsEndpointHostingOptions.MetricsEndpointPort.Value);
                }

                if (metricsEndpointHostingOptions.MetricsTextEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {metricsEndpointHostingOptions.MetricsTextEndpoint} endpoint on port {metricsEndpointHostingOptions.MetricsTextEndpointPort.Value}");
                    ports.Add(metricsEndpointHostingOptions.MetricsTextEndpointPort.Value);
                }

                if (metricsEndpointHostingOptions.EnvironmentInfoEndpointPort.HasValue)
                {
                    Console.WriteLine($"Hosting {metricsEndpointHostingOptions.EnvironmentInfoEndpoint} endpoint on port {metricsEndpointHostingOptions.EnvironmentInfoEndpointPort.Value}");
                    ports.Add(metricsEndpointHostingOptions.EnvironmentInfoEndpointPort.Value);
                }
            }

            if (ports.Any())
            {
                throw new NotImplementedException("To implement setting custom ports for netcore3.0, removed for now in App.Metrics 4.0.0");
                // var existingUrl = hostBuilder.GetSetting(WebHostDefaults.ServerUrlsKey);
                // var additionalUrls = string.Join(";", ports.Distinct().Select(p => $"http://*:{p}/"));
                // hostBuilder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};{additionalUrls}");
            }

            hostBuilder.ConfigureServices((context, services) => services.Configure(setupHostingConfiguration));

            return hostBuilder;
        }
    }
}
