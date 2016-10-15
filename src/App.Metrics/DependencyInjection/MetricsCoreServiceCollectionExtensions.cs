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

        internal static IMetricsBuilder AddMetricsCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return AddMetricsCore(services, setupAction: null, metricsContext: default(IMetricsContext));
        }

        internal static IMetricsBuilder AddMetricsCore(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction,
            IMetricsContext metricsContext)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var metricsEnvironment = new MetricsAppEnvironment(PlatformServices.Default.Application);

            services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();

            services.ConfigureDefaultServices();
            services.AddDefaultHealthCheckServices(metricsEnvironment);
            services.AddDefaultReporterServices();
            services.AddDefaultJsonServices();
            services.AddMetricsCoreServices(metricsEnvironment, metricsContext);            

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new MetricsBuilder(services, metricsEnvironment);
        }

        internal static void AddDefaultHealthCheckServices(this IServiceCollection services,
            IMetricsEnvironment environment)
        {
            services.TryAddSingleton<IHealthCheckRegistry, HealthCheckRegistry>();
            services.TryAddSingleton<IHealthCheckDataProvider, DefaultHealthCheckDataProvider>();

            services.AddHealthChecks(environment);
        }

        internal static void AddDefaultReporterServices(this IServiceCollection services)
        {
            services.TryAddSingleton<StringReport, StringReport>();
            services.TryAddSingleton(typeof(IMetricReporterRegistry), provider =>
            {
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                var context = provider.GetRequiredService<IMetricsContext>();                
                var healthCheckDataProvider = provider.GetRequiredService<IHealthCheckDataProvider>();
                var options = provider.GetRequiredService<IOptions<AppMetricsOptions>>();

                var registry = new MetricReporterRegistry(
                   loggerFactory,
                   context.DataProvider,
                   options.Value.SystemClock,
                   healthCheckDataProvider);

                options.Value.Reporters(registry);

                return registry;
            });
        }

        internal static void AddDefaultJsonServices(this IServiceCollection services)
        {
            services.TryAddSingleton<MetricsJsonBuilderV1, MetricsJsonBuilderV1>();
            services.TryAddSingleton(typeof(IMetricsJsonBuilder), provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppMetricsOptions>>();
                var jsonBuilderType = MetricsJsonBuilderVersionMapping[options.Value.JsonSchemeVersion];
                return provider.GetRequiredService(jsonBuilderType);
            });
        }

        internal static void AddMetricsCoreServices(this IServiceCollection services,
            IMetricsEnvironment environment, IMetricsContext metricsContext)
        {
            services.TryAddSingleton(typeof(IClock), provider => provider.GetRequiredService<IOptions<AppMetricsOptions>>().Value.SystemClock);
            services.TryAddSingleton<EnvironmentInfoBuilder, EnvironmentInfoBuilder>();

            services.TryAddSingleton(typeof(IMetricsContext), provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppMetricsOptions>>();
                var healthCheckRegistry = provider.GetRequiredService<IHealthCheckRegistry>();
                var healthCheckDataProvider = provider.GetRequiredService<IHealthCheckDataProvider>();

                if (!options.Value.DisableHealthChecks)
                {
                    options.Value.HealthCheckRegistry(healthCheckRegistry);
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
                    metricsContext = new DefaultMetricsContext(options.Value.GlobalContextName, options.Value.SystemClock, healthCheckDataProvider);
                }


                if (options.Value.DisableMetrics)
                {
                    metricsContext.Advanced.CompletelyDisableMetrics();
                }

                return metricsContext;
            });

            services.TryAddSingleton(provider => environment);
        }

        private static void ConfigureDefaultServices(this IServiceCollection services)
        {
            services.TryAddSingleton<ILoggerFactory, LoggerFactory>(); //TODO: AH - don't register here
            services.AddOptions();
        }
    }
}