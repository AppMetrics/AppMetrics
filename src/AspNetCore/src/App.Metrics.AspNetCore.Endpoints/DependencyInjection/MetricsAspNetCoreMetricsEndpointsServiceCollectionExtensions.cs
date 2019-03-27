// <copyright file="MetricsAspNetCoreMetricsEndpointsServiceCollectionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Endpoints.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsAspNetCoreMetricsEndpointsServiceCollectionExtensions
    {
        private static readonly string DefaultConfigSection = nameof(MetricEndpointsOptions);

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(this IServiceCollection services)
        {
            AddMetricsEndpointsServices(services);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMetricsEndpoints(configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(
            this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.AddMetricsEndpoints();

            services.Configure<MetricEndpointsOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricEndpointsOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricEndpointsOptions}" /> to configure the provided <see cref="MetricEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MetricEndpointsOptions> setupAction)
        {
            services.AddMetricsEndpoints(configuration.GetSection(DefaultConfigSection), setupAction);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="MetricEndpointsOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricEndpointsOptions}" /> to configure the provided <see cref="MetricEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(
            this IServiceCollection services,
            IConfigurationSection configuration,
            Action<MetricEndpointsOptions> setupAction)
        {
            services.AddMetricsEndpoints();

            services.Configure<MetricEndpointsOptions>(configuration);
            services.Configure(setupAction);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricEndpointsOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(
            this IServiceCollection services,
            Action<MetricEndpointsOptions> setupAction,
            IConfiguration configuration)
        {
            services.AddMetricsEndpoints(setupAction, configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricEndpointsOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="MetricEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(
            this IServiceCollection services,
            Action<MetricEndpointsOptions> setupAction,
            IConfigurationSection configuration)
        {
            services.AddMetricsEndpoints();

            services.Configure(setupAction);
            services.Configure<MetricEndpointsOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsEndpoints(
            this IServiceCollection services,
            Action<MetricEndpointsOptions> setupAction)
        {
            services.AddMetricsEndpoints();

            services.Configure(setupAction);

            return services;
        }

        internal static void AddMetricsEndpointsServices(IServiceCollection services)
        {
            //
            // Options
            //
            var endpointOptionsDescriptor = ServiceDescriptor.Singleton<IConfigureOptions<MetricEndpointsOptions>, MetricsEndpointsOptionsSetup>();
            services.TryAddEnumerable(endpointOptionsDescriptor);

            //
            // Response Writers
            //
            services.TryAddSingleton<IEnvResponseWriter, DefaultEnvResponseWriter>();
            services.TryAddSingleton<IMetricsResponseWriter, DefaultMetricsResponseWriter>();
        }
    }
}
