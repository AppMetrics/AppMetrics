// <copyright file="HealthResultsAsMetricsBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Health.Reporting.Metrics;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class HealthResultsAsMetricsBuilderExtensions
    {
        public static IHealthBuilder ToMetrics(
            this IHealthReportingBuilder healthReportingBuilder,
            IMetrics metrics)
        {
            healthReportingBuilder.Using(new HealthResultsAsMetricsReporter(metrics));

            return healthReportingBuilder.Builder;
        }

        public static IHealthBuilder ToMetrics(
            this IHealthReportingBuilder healthReportingBuilder,
            IMetrics metrics,
            HealthAsMetricsOptions options)
        {
            healthReportingBuilder.Using(new HealthResultsAsMetricsReporter(metrics, options));

            return healthReportingBuilder.Builder;
        }
    }
}