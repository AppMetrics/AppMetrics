using System;
using AspNet.Metrics;
using AspNet.Metrics.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class AspNetMetricsCoreBuilderExtensions
    {
        public static IMetricsHost AddAspNetMetrics(
            this IMetricsHost host)
        {
            host.AddAspNetMetrics(setupAction: null);
            return host;
        }

        public static IMetricsHost AddAspNetMetrics(
            this IMetricsHost host,
            Action<AspNetMetricsOptions> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            ConfigureDefaultServices(host.Services);

            AddAspNetMetricsCoreServices(host.Services);

            if (setupAction != null)
            {
                host.Services.Configure(setupAction);
            }

            return host;
        }

        internal static void AddAspNetMetricsCoreServices(IServiceCollection services)
        {
            services.TryAddSingleton<IConfigureOptions<AspNetMetricsOptions>, AspNetMetricsCoreOptionsSetup>();
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddRouting();
        }
    }
}