using System;
using App.Metrics.Internal;
using AspNet.Metrics;
using AspNet.Metrics.Internal;
using Microsoft.Extensions.Options;

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
            Action<AspNetMetricsOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ConfigureDefaultServices(services);

            AddMetricsCoreServices(services);

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new MetricsBuilder(services);
        }

        internal static void AddMetricsCoreServices(IServiceCollection services)
        {
            services.TryAddSingleton<IConfigureOptions<AspNetMetricsOptions>, AspNetMetricsCoreOptionsSetup>();
            services.TryAddSingleton<MetricsMarkerService, MetricsMarkerService>();
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddOptions();
        }
    }
}