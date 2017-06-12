// <copyright file="AppMetricsCoreLoggerExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
{
    // ReSharper restore CheckNamespace
    [ExcludeFromCodeCoverage]
    internal static class AppMetricsCoreLoggerExtensions
    {
        internal static void GettingMetricsData(this ILogger logger) { logger.LogTrace(AppMetricsEventIds.Metrics.Data, "GettingMetricsData"); }

        internal static void RetrievedMetricsData(this ILogger logger) { logger.LogTrace(AppMetricsEventIds.Metrics.Data, "RetrievedMetricsData"); }

        private static class AppMetricsEventIds
        {
            private const int MetricsStart = 2000;

            public static class Metrics
            {
                public const int Data = MetricsStart + 1;
            }

            // ReSharper disable UnusedMember.Local
            public static class Reports
            {
                public const int Schedule = MetricsStart + 2;
            }

            // ReSharper restore UnusedMember.Local
        }
    }
}