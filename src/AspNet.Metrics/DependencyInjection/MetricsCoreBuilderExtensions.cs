using System;
using System.Linq;
using AspNet.Metrics;
using AspNet.Metrics.Infrastructure;
using AspNet.Metrics.Internal;
using Metrics;
using Metrics.Reports;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsCoreBuilderExtensions
    {
        public static IMetricsBuilder AddHealthChecks(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var environment = GetServiceFromCollection<IHostingEnvironment>(builder.Services);
            if (environment == null)
            {
                return builder;
            }

            HealthChecksAsServices.AddHealthChecksAsServices(builder.Services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(environment.ApplicationName));

            return builder;
        }

        public static IMetricsBuilder AddMetricsOptions(
            this IMetricsBuilder builder,
            Action<MetricsOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure<MetricsOptions>(setupAction);
            return builder;
        }

        public static IMetricsBuilder AddReporter(this IMetricsBuilder builder, Action<MetricsReports> reportsAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Metric.Config.WithReporting(reportsAction);

            return builder;
        }

        private static T GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }
    }
}