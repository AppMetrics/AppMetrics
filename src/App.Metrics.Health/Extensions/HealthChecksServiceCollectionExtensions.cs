// <copyright file="HealthChecksServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Builder;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class HealthChecksServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <returns>The health check checksBuilder</returns>
        // ReSharper disable MemberCanBePrivate.Global
        public static IAppMetricsHealthChecksBuilder AddHealthChecks(this IServiceCollection services)
            // ReSharper restore MemberCanBePrivate.Global
        {
            var builder = services.AddHealthBuilder();

            builder.AddRequiredPlatformServices();

            builder.AddCoreServices();

            return builder;
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load <see cref="AppMetricsHealthOptions">options</see>.
        /// </param>
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppMetricsHealthOptions>(configuration);
            return services.AddHealthChecks();
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupAction">The <see cref="AppMetricsHealthOptions">options</see> setup action.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsHealthOptions">options</see>. Any shared configuration options with the options delegate will be
        ///     overridden by using this configuration.
        /// </param>
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealthChecks(
            this IServiceCollection services,
            Action<AppMetricsHealthOptions> setupAction,
            IConfiguration configuration)
        {
            services.Configure(setupAction);
            services.Configure<AppMetricsHealthOptions>(configuration);
            return services.AddHealthChecks();
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsHealthOptions">options</see>.
        /// </param>
        /// <param name="setupAction">The <see cref="AppMetricsHealthOptions">options</see> setup action.</param>
        /// Any shared configuration options with the options IConfiguration will be overriden by the options delegate.
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsHealthOptions> setupAction)
        {
            services.Configure<AppMetricsHealthOptions>(configuration);
            services.Configure(setupAction);
            return services.AddHealthChecks();
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupAction">The <see cref="AppMetricsHealthOptions">options</see> setup action.</param>
        /// <returns>The health check checksBuilder</returns>
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IAppMetricsHealthChecksBuilder AddHealthChecks(this IServiceCollection services, Action<AppMetricsHealthOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddHealthChecks();
        }

        /// <summary>
        ///     Adds the health check services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <returns>The health check checksBuilder</returns>
        // ReSharper disable MemberCanBePrivate.Global
        internal static IAppMetricsHealthChecksBuilder AddHealthBuilder(this IServiceCollection services) { return new AppMetricsHealthChecksBuilder(services); }
        // ReSharper restore MemberCanBePrivate.Global
    }
}