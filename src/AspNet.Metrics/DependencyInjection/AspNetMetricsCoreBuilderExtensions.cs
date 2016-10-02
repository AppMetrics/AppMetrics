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
        public static IMetricsBuilder AddAspNetMetrics(
            this IMetricsBuilder builder)
        {
            builder.AddAspNetMetrics(setupAction: null);
            return builder;
        }

        public static IMetricsBuilder AddAspNetMetrics(
            this IMetricsBuilder builder,
            Action<AspNetMetricsOptions> setupAction)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            ConfigureDefaultServices(builder.Services);

            AddAspNetMetricsCoreServices(builder.Services);

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }

            return builder;
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