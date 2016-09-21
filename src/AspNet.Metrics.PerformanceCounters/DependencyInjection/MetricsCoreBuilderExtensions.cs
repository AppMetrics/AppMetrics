using System;
using Metrics;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public static class MetricsCoreBuilderExtensions
    {
        public static IMetricsBuilder WithAllPerformanceCounters(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Metric.Config.WithSystemCounters();
            Metric.Config.WithAppCounters();
            Metric.Config.WithAspNetPerformanceCounters();
            Metric.Config.WithSqlServerPerformanceCounters();

            return builder;
        }

        public static IMetricsBuilder WithApplicationPerforrmanceCounters(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Metric.Config.WithAppCounters();

            return builder;
        }

        public static IMetricsBuilder WithAspNetPerforrmanceCounters(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Metric.Config.WithAspNetPerformanceCounters();

            return builder;
        }

        public static IMetricsBuilder WithSqlServerPerformanceCounters(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Metric.Config.WithSqlServerPerformanceCounters();

            return builder;
        }

        public static IMetricsBuilder WithSystemPerforrmanceCounters(this IMetricsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Metric.Config.WithSystemCounters();

            return builder;
        }
    }
}