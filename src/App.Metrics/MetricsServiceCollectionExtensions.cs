// <copyright file="MetricsServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Filtering;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods for setting up App Metrics services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class MetricsServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsBuilder AddMetrics(this IServiceCollection services)
        {
            var builder = services.AddMetricsCore();

            // Add default framework
            builder.AddGlobalFilter(new NoOpMetricsFilter());
            builder.AddClockType<StopwatchClock>();
            builder.AddDefaultReservoir(() => new DefaultForwardDecayingReservoir());

            return new MetricsBuilder(services);
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsBuilder AddMetrics(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = services.AddMetrics();

            services.Configure<MetricsOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsOptions}" /> to configure the provided <see cref="MetricsOptions" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsBuilder AddMetrics(
            this IServiceCollection services,
            Action<MetricsOptions> setupAction,
            IConfiguration configuration)
        {
            var builder = services.AddMetrics();

            services.Configure(setupAction);
            services.Configure<MetricsOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsOptions" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsOptions}" /> to configure the provided <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsBuilder AddMetrics(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MetricsOptions> setupAction)
        {
            var builder = services.AddMetrics();

            services.Configure<MetricsOptions>(configuration);
            services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds the metrics services and configuration to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsOptions}" /> to configure the provided <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsBuilder AddMetrics(this IServiceCollection services, Action<MetricsOptions> setupAction)
        {
            var builder = services.AddMetrics();

            services.Configure(setupAction);

            return builder;
        }
    }
}