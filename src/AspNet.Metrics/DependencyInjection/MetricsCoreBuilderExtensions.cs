using System;
using AspNet.Metrics;
using AspNet.Metrics.Infrastructure;
using AspNet.Metrics.Internal;
using Metrics;
using Metrics.Reports;
using Microsoft.Extensions.PlatformAbstractions;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsCoreBuilderExtensions
    {
        public static IMetricsBuilder AddAllPerforrmanceCounters(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Metric.Config.WithAllCounters();

            return builder;
        }

        public static IMetricsBuilder AddHealthChecks(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }


            var libraryManager = PlatformServices.Default?.LibraryManager;
            var assemblyProvider = new DefaultMetricsAssemblyProvider(libraryManager);
            HealthChecksAsServices.AddHealthChecksAsServices(builder.Services, assemblyProvider.CandidateAssemblies);

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
    }
}