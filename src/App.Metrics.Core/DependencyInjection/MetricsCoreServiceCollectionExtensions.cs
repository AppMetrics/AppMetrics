// <copyright file="MetricsCoreServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Internal.NoOp;
using App.Metrics.Registry;
using App.Metrics.ReservoirSampling;
using App.Metrics.Tagging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods for setting up essential App Metrics services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class MetricsCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsCoreBuilder AddMetricsCore(this IServiceCollection services)
        {
            AddMetricsCoreServices(services);

            var builder = new MetricsCoreBuilder(services);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics services and configuration to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsCoreBuilder AddMetricsCore(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = services.AddMetricsCore();

            services.Configure<MetricsOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics services and configuration to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsOptions}" /> to configure the provided <see cref="MetricsOptions" />.
        /// </param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsCoreBuilder AddMetricsCore(
            this IServiceCollection services,
            Action<MetricsOptions> setupAction,
            IConfiguration configuration)
        {
            var builder = services.AddMetricsCore();

            services.Configure(setupAction);
            services.Configure<MetricsOptions>(configuration);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics services and configuration to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> from where to load <see cref="MetricsOptions" />.
        /// </param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsOptions}" /> to configure the provided <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsCoreBuilder AddMetricsCore(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MetricsOptions> setupAction)
        {
            var builder = services.AddMetricsCore();

            services.Configure<MetricsOptions>(configuration);
            services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        ///     Adds essential App Metrics services and configuration to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsOptions}" /> to configure the provided <see cref="MetricsOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsCoreBuilder" /> that can be used to further configure the App Metrics services.
        /// </returns>
        public static IMetricsCoreBuilder AddMetricsCore(this IServiceCollection services, Action<MetricsOptions> setupAction)
        {
            var builder = services.AddMetricsCore();

            services.Configure(setupAction);

            return builder;
        }

        internal static void AddMetricsCoreServices(IServiceCollection services)
        {
            //
            // Options
            //
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MetricsOptions>, MetricsCoreMetricsOptionsSetup>());

            //
            // Metrics Registry
            //
            services.TryAddTransient<Func<string, IMetricContextRegistry>>(
                provider =>
                {
                    var optionsAccessor = provider.GetRequiredService<IOptions<MetricsOptions>>();
                    var globalTags = optionsAccessor.Value.GlobalTags;

                    return context => new DefaultMetricContextRegistry(context, new GlobalMetricTags(globalTags));
                });

            services.TryAddSingleton<IMetricsRegistry>(
                provider =>
                {
                    var optionsAccessor = provider.GetRequiredService<IOptions<MetricsOptions>>();

                    if (!optionsAccessor.Value.MetricsEnabled)
                    {
                        return new NullMetricsRegistry();
                    }

                    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                    var clock = provider.GetRequiredService<IClock>();
                    var newContextRegistry = provider.GetRequiredService<Func<string, IMetricContextRegistry>>();

                    return new DefaultMetricsRegistry(loggerFactory, optionsAccessor, clock, newContextRegistry);
                });

            //
            // Sampling
            //
            services.TryAddSingleton(provider => new DefaultSamplingReservoirProvider());

            //
            // Filtering
            //
            services.TryAddSingleton<IFilterMetrics, DefaultMetricsFilter>();

            //
            // Metrics
            //
            services.TryAddSingleton<IMeasureMetrics, DefaultMeasureMetricsProvider>();
            services.TryAddSingleton<IBuildMetrics, DefaultMetricsBuilderFactory>();
            services.TryAddSingleton<IProvideMetrics, DefaultMetricsProvider>();
            services.TryAddSingleton<IProvideMetricValues, DefaultMetricValuesProvider>();
            services.TryAddSingleton<IManageMetrics, DefaultMetricsManager>();
            services.TryAddSingleton<IMetrics, DefaultMetrics>();

            //
            // Random Infrasturcture
            //
            services.TryAddSingleton<AppMetricsMarkerService, AppMetricsMarkerService>();
            services.TryAddSingleton<EnvironmentInfoProvider, EnvironmentInfoProvider>();
            services.TryAddSingleton<IClock>(provider => new StopwatchClock());
        }
    }
}