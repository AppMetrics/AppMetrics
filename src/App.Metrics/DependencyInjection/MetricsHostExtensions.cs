using App.Metrics;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsHostExtensions
    {
        public static IMetricsHostBuilder AddGlobalFilter(this IMetricsHostBuilder host, IMetricsFilter filter)
        {
            host.Services.Replace(ServiceDescriptor.Singleton(filter));

            return host;
        }
    }
}