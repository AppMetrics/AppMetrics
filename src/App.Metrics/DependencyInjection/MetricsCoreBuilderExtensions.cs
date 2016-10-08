using System;
using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    internal static class MetricsCoreBuilderExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IMetricsEnvironment environment)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            HealthChecksAsServices.AddHealthChecksAsServices(services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(environment.ApplicationName));

            return services;
        }
    }
}