using System;
using App.Metrics;
using App.Metrics.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions
// ReSharper restore CheckNamespace
{
    public static class MetricsCoreServiceCollectionExtensions
    {
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
            services.TryAddSingleton<IConfigureOptions<AppMetricsOptions>, AppMetricsCoreOptionsSetup>();
            services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
            services.TryAddSingleton(provider => environment);
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}