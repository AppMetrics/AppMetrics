// <copyright file="MetricsAspNetCoreMetricsTrackingMiddlewareServiceCollectionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.AspNetCore.Tracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsAspNetCoreMetricsTrackingMiddlewareServiceCollectionExtensions
    {
        private static readonly string DefaultConfigSection = nameof(MetricsWebTrackingOptions);

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(this IServiceCollection services)
         {
             AddMetricsTrackingMiddlewareServices(services);

            return services;
        }

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMetricsTrackingMiddleware(configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(
            this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.AddMetricsTrackingMiddleware();

            services.Configure<MetricsWebTrackingOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsWebTrackingOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsWebTrackingOptions}" /> to configure the provided <see cref="MetricsWebTrackingOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MetricsWebTrackingOptions> setupAction)
        {
            services.AddMetricsTrackingMiddleware(configuration.GetSection(DefaultConfigSection), setupAction);

            return services;
        }

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="MetricsWebTrackingOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsWebTrackingOptions}" /> to configure the provided <see cref="MetricsWebTrackingOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(
            this IServiceCollection services,
            IConfigurationSection configuration,
            Action<MetricsWebTrackingOptions> setupAction)
        {
            services.AddMetricsTrackingMiddleware();

            services.Configure<MetricsWebTrackingOptions>(configuration);
            services.Configure(setupAction);

            return services;
        }

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsWebTrackingOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(
            this IServiceCollection services,
            Action<MetricsWebTrackingOptions> setupAction,
            IConfiguration configuration)
        {
            services.AddMetricsTrackingMiddleware(setupAction, configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsWebTrackingOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="MetricsWebTrackingOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(
            this IServiceCollection services,
            Action<MetricsWebTrackingOptions> setupAction,
            IConfigurationSection configuration)
        {
            services.AddMetricsTrackingMiddleware();

            services.Configure(setupAction);
            services.Configure<MetricsWebTrackingOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds App Metrics AspNet Core metrics tracking middleware services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsAspNetCoreOptions}" /> to configure the provided <see cref="MetricsWebTrackingOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddMetricsTrackingMiddleware(
            this IServiceCollection services,
            Action<MetricsWebTrackingOptions> setupAction)
        {
            services.AddMetricsTrackingMiddleware();

            services.Configure(setupAction);

            return services;
        }

        internal static void AddMetricsTrackingMiddlewareServices(IServiceCollection services)
        {
            //
            // Options
            //
            var descriptor = ServiceDescriptor.Singleton<IConfigureOptions<MetricsWebTrackingOptions>, MetricsTrackingMiddlewareOptionsSetup>();
            services.TryAddEnumerable(descriptor);
        }
    }
}
