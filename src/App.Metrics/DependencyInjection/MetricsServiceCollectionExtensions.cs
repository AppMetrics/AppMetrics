using System;
using App.Metrics;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsServiceCollectionExtensions
    {
        public static IMetricsBuilder AddMetrics(this IServiceCollection services)
        {
            return services.AddMetricsCore(null);
        }

        public static IMetricsBuilder AddMetrics(this IServiceCollection services,
            Action<AppMetricsOptions> setupAction)
        {
            var builder = services.AddMetricsCore(setupAction);

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }

            return builder;
        }
    }
}