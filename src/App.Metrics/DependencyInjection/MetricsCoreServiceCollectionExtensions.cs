using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.Internal;
using App.Metrics.Json;
using App.Metrics.Reporters;
using App.Metrics.Utils;
using AspNet.Metrics;
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

            return AddMetricsCore(services, setupAction: null);
        }

        public static IMetricsBuilder AddMetricsCore(
            this IServiceCollection services,
            Action<AppMetricsOptions> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var metricsEnvironment = new MetricsAppEnvironment(PlatformServices.Default.Application);

            ConfigureDefaultServices(services);

            AddMetricsCoreServices(services, metricsEnvironment);

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new MetricsBuilder(services, metricsEnvironment);
        }

        internal static void AddMetricsCoreServices(IServiceCollection services, IMetricsEnvironment environment)
        {
            //TODO: AH - is this still needed
            services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
            services.TryAddSingleton<IConfigureOptions<AppMetricsOptions>, AppMetricsCoreOptionsSetup>();
            services.TryAddSingleton<AppEnvironment, AppEnvironment>();
            services.TryAddSingleton<MetricsJsonBuilderV1, MetricsJsonBuilderV1>();
            services.TryAddSingleton<MetricsErrorHandler, MetricsErrorHandler>();
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
                //TODO: AH - refactor error handling
                var errorHandler = provider.GetRequiredService<MetricsErrorHandler>();
                var reporters = new MetricsReports(
                    loggerFactory,                    
                    options.Value.MetricsContext.DataProvider,
                    errorHandler,
                    options.Value.MetricsContext.HealthStatus);

                options.Value.Reporters(reporters);

                if (options.Value.EnableInternalMetrics)
                {
                    //TODO: Review enableing internal metrics
                    var internalMetricsContexxt = new DefaultMetricsContext(BaseMetricsContext.InternalMetricsContextName, options.Value.SystemClock);
                    options.Value.MetricsContext.Advanced.AttachContext(BaseMetricsContext.InternalMetricsContextName,
                        internalMetricsContexxt);
                }

                return options.Value.MetricsContext;
            });

            services.TryAddSingleton(provider => environment);
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}