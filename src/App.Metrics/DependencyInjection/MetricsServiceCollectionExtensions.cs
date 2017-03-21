// <copyright file="MetricsServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Configuration;
using App.Metrics.Core.Internal;
using Microsoft.Extensions.Configuration;

#if NET452
using System.Reflection;
#endif

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsServiceCollectionExtensions
    {
#if NET452
/// <summary>
///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
/// </summary>
/// <param name="services">The application services collection.</param>
/// <param name="entryAssemblyName">The application assembly name</param>
/// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: Excluding for now, don't think the it's worth the effort in testing net452 at this time.
        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services, AssemblyName entryAssemblyName)
        {
            var builder = services.AddMetricsHostBuilder(entryAssemblyName);

            builder.AddRequiredPlatformServices();

            builder.AddCoreServices();

            return new MetricsHostBuilder(services, entryAssemblyName);
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load <see cref="AppMetricsOptions">options</see>.
        /// </param>
        /// <param name="entryAssemblyName">The application assembly name</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services, IConfiguration configuration, AssemblyName entryAssemblyName)
        {
            services.Configure<AppMetricsOptions>(configuration);
            return services.AddMetrics(entryAssemblyName);
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupAction">The <see cref="AppMetricsOptions">options</see> setup action.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsOptions">options</see>. Any shared configuration options with the options delegate will be
        ///     overriden by using this configuration.
        /// </param>
        /// <param name="entryAssemblyName">The application assembly name</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddMetrics(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction,
            IConfiguration configuration,
            AssemblyName entryAssemblyName)
        {
            services.Configure(setupAction);
            services.Configure<AppMetricsOptions>(configuration);
            return services.AddMetrics(entryAssemblyName);
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsOptions">options</see>.
        /// </param>
        /// <param name="setupAction">
        ///     The <see cref="AppMetricsOptions">options</see> setup action.
        /// </param>
        /// Any shared configuration options with the options IConfiguration will be overriden by the options delegate.
        /// <param name="entryAssemblyName">The application assembly name</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddMetrics(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsOptions> setupAction,
            AssemblyName entryAssemblyName)
        {
            services.Configure<AppMetricsOptions>(configuration);
            services.Configure(setupAction);
            return services.AddMetrics(entryAssemblyName);
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupAction">The <see cref="AppMetricsOptions">options</see> setup action.</param>
        /// <param name="entryAssemblyName">The application assembly name</param>
        /// <returns>The metrics host builder</returns>
        public static IMetricsHostBuilder AddMetrics(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction,
            AssemblyName entryAssemblyName)
        {
            services.Configure(setupAction);
            return services.AddMetrics(entryAssemblyName);
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="entryAssemblyName">The application assembly name</param>
        /// <returns>The metrics host builder</returns>
        internal static IMetricsHostBuilder AddMetricsHostBuilder(this IServiceCollection services, AssemblyName entryAssemblyName)
        {
            return new MetricsHostBuilder(services, entryAssemblyName);
        }
#else

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <returns>The metrics host builder</returns>
        // ReSharper disable MemberCanBePrivate.Global
        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services)
            // ReSharper restore MemberCanBePrivate.Global
        {
            var builder = services.AddMetricsHostBuilder();

            builder.AddRequiredPlatformServices();

            builder.AddCoreServices();

            return new MetricsHostBuilder(services);
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load <see cref="AppMetricsOptions">options</see>.
        /// </param>
        /// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppMetricsOptions>(configuration);
            return services.AddMetrics();
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupAction">The <see cref="AppMetricsOptions">options</see> setup action.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsOptions">options</see>. Any shared configuration options with the options delegate will be
        ///     overriden by using this configuration.
        /// </param>
        /// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IMetricsHostBuilder AddMetrics(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction,
            IConfiguration configuration)
        {
            services.Configure(setupAction);
            services.Configure<AppMetricsOptions>(configuration);
            return services.AddMetrics();
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration">IConfiguration</see> from where to load
        ///     <see cref="AppMetricsOptions">options</see>.
        /// </param>
        /// <param name="setupAction">The <see cref="AppMetricsOptions">options</see> setup action.</param>
        /// Any shared configuration options with the options IConfiguration will be overriden by the options delegate.
        /// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IMetricsHostBuilder AddMetrics(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<AppMetricsOptions> setupAction)
        {
            services.Configure<AppMetricsOptions>(configuration);
            services.Configure(setupAction);
            return services.AddMetrics();
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <param name="setupAction">The <see cref="AppMetricsOptions">options</see> setup action.</param>
        /// <returns>The metrics host builder</returns>
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IMetricsHostBuilder AddMetrics(this IServiceCollection services, Action<AppMetricsOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddMetrics();
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services">The application services collection.</param>
        /// <returns>The metrics host builder</returns>
        // ReSharper disable MemberCanBePrivate.Global
        internal static IMetricsHostBuilder AddMetricsHostBuilder(this IServiceCollection services) { return new MetricsHostBuilder(services); }
        // ReSharper restore MemberCanBePrivate.Global
#endif
    }
}