// <copyright file="HealthAspNetCoreHealthEndpointsServiceCollectionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.AspNetCore.Health;
using App.Metrics.AspNetCore.Health.Core;
using App.Metrics.AspNetCore.Health.Core.Internal.NoOp;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.AspNetCore.Health.Endpoints.Internal;
using App.Metrics.Health;
using App.Metrics.Health.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class HealthAspNetCoreHealthEndpointsServiceCollectionExtensions
    {
        private static readonly string DefaultConfigSection = nameof(HealthEndpointsOptions);

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(this IServiceCollection services)
        {
            AddHealthEndpointsServices(services);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthEndpoints(configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="HealthEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.AddHealthEndpoints();

            services.Configure<HealthEndpointsOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core health services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthEndpointsOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointsOptions}" /> to configure the provided <see cref="HealthEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<HealthEndpointsOptions> setupAction)
        {
            services.AddHealthEndpoints(configuration.GetSection(DefaultConfigSection), setupAction);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="HealthEndpointsOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointsOptions}" /> to configure the provided <see cref="HealthEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            IConfigurationSection configuration,
            Action<HealthEndpointsOptions> setupAction)
        {
            services.AddHealthEndpoints();

            services.Configure<HealthEndpointsOptions>(configuration);
            services.Configure(setupAction);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointsOptions}" /> to configure the provided <see cref="HealthEndpointsOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="HealthEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            Action<HealthEndpointsOptions> setupAction,
            IConfiguration configuration)
        {
            services.AddHealthEndpoints(setupAction, configuration.GetSection(DefaultConfigSection));

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointsOptions}" /> to configure the provided <see cref="HealthEndpointsOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfigurationSection" /> from where to load <see cref="HealthEndpointsOptions" />.</param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            Action<HealthEndpointsOptions> setupAction,
            IConfigurationSection configuration)
        {
            services.AddHealthEndpoints();

            services.Configure(setupAction);
            services.Configure<HealthEndpointsOptions>(configuration);

            return services;
        }

        /// <summary>
        ///     Adds essential App Metrics Health AspNet Core metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{HealthEndpointsOptions}" /> to configure the provided <see cref="HealthEndpointsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IServiceCollection" /> that can be used to further configure services.
        /// </returns>
        public static IServiceCollection AddHealthEndpoints(
            this IServiceCollection services,
            Action<HealthEndpointsOptions> setupAction)
        {
            services.AddHealthEndpoints();

            services.Configure(setupAction);

            return services;
        }

        internal static void AddHealthEndpointsServices(IServiceCollection services)
        {
            var endpointOptionsDescriptor = ServiceDescriptor.Singleton<IConfigureOptions<HealthEndpointsOptions>, HealthEndpointsOptionsSetup>();
            services.TryAddEnumerable(endpointOptionsDescriptor);

            services.TryAddSingleton(provider => ResolveHealthResponseWriter(provider));
        }

        internal static IHealthResponseWriter ResolveHealthResponseWriter(IServiceProvider provider, IHealthOutputFormatter formatter = null)
        {
            var endpointOptions = provider.GetRequiredService<IOptions<HealthEndpointsOptions>>();
            var health = provider.GetRequiredService<IHealthRoot>();

            if (health.Options.Enabled && endpointOptions.Value.HealthEndpointEnabled && health.OutputHealthFormatters.Any())
            {
                if (formatter == null)
                {
                    var fallbackFormatter = endpointOptions.Value.HealthEndpointOutputFormatter ?? health.DefaultOutputHealthFormatter;

                    return new DefaultHealthResponseWriter(fallbackFormatter, health.OutputHealthFormatters);
                }

                return new DefaultHealthResponseWriter(formatter, health.OutputHealthFormatters);
            }

            return new NoOpHealthStatusResponseWriter();
        }
    }
}
