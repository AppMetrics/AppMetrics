using AspNet.Metrics.Configuration;
using AspNet.Metrics.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsHostBuilderExtensionsAspNetCore
    {
        public static IMetricsHostBuilder AddRequiredAspNetPlatformServices(this IMetricsHostBuilder builder)
        {
            builder.Services.TryAddSingleton<AspNetMetricsMarkerService, AspNetMetricsMarkerService>();
            builder.Services.AddRouting();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AspNetMetricsOptions>>().Value);

            return builder;
        }

        internal static void AddAspNetCoreServices(this IMetricsHostBuilder builder)
        {
        }
    }
}