using System;
using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Reporters;

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

            HealthChecksAsServices.AddHealthChecksAsServices(builder.Services,
                DefaultMetricsAssemblyDiscoveryProvider.DiscoverAssemblies(builder.Environment.ApplicationName));

            return builder;
        }

        public static IMetricsBuilder AddMetricsOptions(
            this IMetricsBuilder builder,
            Action<AppMetricsOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure<AppMetricsOptions>(setupAction);
            return builder;
        }

        public static IMetricsBuilder AddReporter(this IMetricsBuilder builder,
            Action<MetricsReports> reportsAction, MetricsConfig config)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            config.WithReporting(reportsAction);

            return builder;
        }

        public static IMetricsBuilder AddReporter(this IMetricsBuilder builder,
            Action<MetricsReports> reportsAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddReporter(reportsAction, Metric.Config);

            return builder;
        }
    }
}