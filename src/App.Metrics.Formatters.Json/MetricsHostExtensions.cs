using App.Metrics.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Metrics.Formatters.Json
{
    public static class MetricsHostExtensions
    {
        public static IMetricsHost AddJsonSerialization(this IMetricsHost host)
        {
            AddJsonNetCoreServices(host.Services);

            return host;
        }

        internal static void AddJsonNetCoreServices(IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Transient<IMetricDataSerializer, MetricDataSerializer>());
            services.Replace(ServiceDescriptor.Transient<IHealthStatusSerializer, HealthStatusSerializer>());
        }
    }
}