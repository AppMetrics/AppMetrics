using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Json;
using App.Metrics.Registries;
using App.Metrics.Reporters;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions
// ReSharper restore CheckNamespace
{
    internal static class MetricsCoreServiceCollectionExtensions
    {
        private static readonly IReadOnlyDictionary<JsonSchemeVersion, Type> MetricsJsonBuilderVersionMapping =
            new ReadOnlyDictionary<JsonSchemeVersion, Type>(new Dictionary<JsonSchemeVersion, Type>
            {
                { JsonSchemeVersion.AlwaysLatest, typeof(MetricsJsonBuilderV1) },
                { JsonSchemeVersion.Version1, typeof(MetricsJsonBuilderV1) }
            });

        public static IMetricsBuilder AddMetricsCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return AddMetricsCore(services, setupAction: null, metricsContext: default(IMetricsContext));
        }

        public static IMetricsBuilder AddMetricsCore(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction,
            IMetricsContext metricsContext)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var metricsEnvironment = new MetricsAppEnvironment(PlatformServices.Default.Application);

            ConfigureDefaultServices(services);

            AddMetricsCoreServices(services, metricsEnvironment, metricsContext);

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new MetricsBuilder(services, metricsEnvironment);
        }

        internal static void AddMetricsCoreServices(IServiceCollection services,
            IMetricsEnvironment environment, IMetricsContext metricsContext)
        {
            services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
            services.TryAddSingleton(typeof(IClock), provider => Clock.Default);
            services.TryAddSingleton<EnvironmentInfoBuilder, EnvironmentInfoBuilder>();
            services.TryAddSingleton<MetricsJsonBuilderV1, MetricsJsonBuilderV1>();
            services.TryAddSingleton<IHealthCheckRegistry, HealthCheckRegistry>();
            services.TryAddSingleton<IHealthCheckDataProvider, HealthCheckDataProvider>();
            services.TryAddSingleton<ILoggerFactory, LoggerFactory>(); //TODO: AH - don't register here
            services.AddHealthChecks(environment);
            services.TryAddSingleton<StringReport, StringReport>();

            services.TryAddSingleton(typeof(IMetricsJsonBuilder), provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppMetricsOptions>>();
                var jsonBuilderType = MetricsJsonBuilderVersionMapping[options.Value.JsonSchemeVersion];
                return provider.GetRequiredService(jsonBuilderType);
            });

            services.TryAddSingleton(typeof(IMetricsContext), provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppMetricsOptions>>();
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var healthCheckRegistry = provider.GetRequiredService<IHealthCheckRegistry>();
                var healthCheckDataProvider = provider.GetRequiredService<IHealthCheckDataProvider>();

                if (!options.Value.DisableHealthChecks)
                {
                    options.Value.HealthCheckRegistry(healthCheckRegistry);

                    //if (healthChecks != null && healthChecks.Any())
                    //{
                    //    foreach (var check in healthChecks)
                    //    {
                    //        healthCheckRegistry.Register(check);
                    //    }
                    //}
                }

                if (options.Value.EnableInternalMetrics)
                {
                    //TODO: Review enableing internal metrics
                    //var internalMetricsContexxt = new DefaultMetricsContext(BaseMetricsContext.InternalMetricsContextName, options.Value.SystemClock);
                    //options.Value.MetricsContext.Advanced.AttachContext(BaseMetricsContext.InternalMetricsContextName,
                    //    internalMetricsContexxt);
                }

                if (metricsContext == default(IMetricsContext))
                {
                    metricsContext = new DefaultMetricsContext(options.Value.GlobalContextName,
                        options.Value.SystemClock, healthCheckDataProvider);
                }


                if (options.Value.DisableMetrics)
                {
                    metricsContext.Advanced.CompletelyDisableMetrics();
                }

                var reporters = new MetricsReports(
                    loggerFactory,
                    metricsContext.DataProvider,
                    metricsContext.GetHealthStatusAsync);

                options.Value.Reporters(reporters);

                return metricsContext;
            });

            services.TryAddSingleton(provider => environment);
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}