// <copyright file="AppMetricsCoreLoggerExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using App.Metrics.Core.Internal;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
// ReSharper disable RedundantStringInterpolation
// ReSharper disable UnusedMember.Local
namespace Microsoft.Extensions.Logging
{
    // ReSharper restore CheckNamespace
    [AppMetricsExcludeFromCodeCoverage]
    internal static class AppMetricsCoreLoggerExtensions
    {
        static AppMetricsCoreLoggerExtensions()
        {
            _healthCheckRegistered = LoggerMessage.Define<string>(
                LogLevel.Trace,
                AppMetricsEventIds.HealthChecks.Registration,
                $"Health Check Registered: {{name}}");
            _healthGetStatusExecuted = LoggerMessage.Define<double, int>(
                LogLevel.Information,
                AppMetricsEventIds.HealthChecks.Status,
                $"Executed {nameof(HealthStatus)}, in {{elapsedMilliseconds}}ms, IsHealthy: True. {{checksPassed}} health check results passed.");

            _healthGetStatusExecutedFailed = LoggerMessage.Define<double, int, int, int, IEnumerable<string>, IEnumerable<string>>(
                LogLevel.Information,
                AppMetricsEventIds.HealthChecks.Status,
                $"Executed {nameof(HealthStatus)}, in {{elapsedMilliseconds}}ms, IsHealthy: False. {{checksPassed}} health check results passed. {{checksFailed}} health check results failed. Failed Checks: {{failedChecks}}. {{checksDegraded}} health check results degredated. Degraded Checks: {{degredatedChecks}}");

            _healthGetStatusExecutedNoResults = LoggerMessage.Define(
                LogLevel.Information,
                AppMetricsEventIds.HealthChecks.Status,
                $"Executed {nameof(HealthStatus)}, 0 health check results.");
        }

        internal static void GettingMetricsData(this ILogger logger) { logger.LogTrace(AppMetricsEventIds.Metrics.Data, "GettingMetricsData"); }

        internal static void HealthCheckGetStatusExecuted(
            this ILogger logger,
            HealthStatus healthStatus,
            long startTimestamp)
        {
            if (!logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

            if (startTimestamp == 0)
            {
                return;
            }

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            if (healthStatus.HasRegisteredChecks)
            {
                if (healthStatus.Status.IsHealthy())
                {
                    _healthGetStatusExecuted(logger, elapsed.TotalMilliseconds, healthStatus.Results.Length, null);
                    return;
                }

                var checksFailed = healthStatus.Results.Count(x => x.Check.Status.IsUnhealthy());
                var checksDegraded = healthStatus.Results.Count(x => x.Check.Status.IsDegraded());
                var checksPassed = healthStatus.Results.Count(x => x.Check.Status.IsHealthy());
                var failedChecks = healthStatus.Results.Where(h => h.Check.Status.IsUnhealthy()).Select(h => h.Name);
                var degradedChecks = healthStatus.Results.Where(h => h.Check.Status.IsDegraded()).Select(h => h.Name);

                _healthGetStatusExecutedFailed(
                    logger,
                    elapsed.TotalMilliseconds,
                    checksFailed,
                    checksDegraded,
                    checksPassed,
                    degradedChecks,
                    failedChecks,
                    null);
            }

            _healthGetStatusExecutedNoResults(logger, null);
        }

        internal static void HealthCheckGetStatusExecuting(this ILogger logger)
        {
            logger.LogTrace(AppMetricsEventIds.HealthChecks.Status, "Executing HealthCheck Get Status");
        }

        internal static void HealthCheckRegistered(this ILogger logger, string name) { _healthCheckRegistered(logger, name, null); }

        internal static void RetrievedMetricsData(this ILogger logger) { logger.LogTrace(AppMetricsEventIds.Metrics.Data, "RetrievedMetricsData"); }

        private static class AppMetricsEventIds
        {
            private const int MetricsStart = 2000;

            public static class HealthChecks
            {
                public const int Registration = MetricsStart + 3;
                public const int Status = MetricsStart + 3;
            }

            public static class Metrics
            {
                public const int Data = MetricsStart + 1;
            }

            public static class Reports
            {
                public const int Schedule = MetricsStart + 2;
            }
        }
#pragma warning disable SA1201

        // ReSharper disable InconsistentNaming
        private static readonly Action<ILogger, string, Exception> _healthCheckRegistered;
        private static readonly Action<ILogger, double, int, Exception> _healthGetStatusExecuted;

        private static readonly Action<ILogger, double, int, int, int, IEnumerable<string>, IEnumerable<string>, Exception>
            _healthGetStatusExecutedFailed;

        private static readonly Action<ILogger, Exception> _healthGetStatusExecutedNoResults;
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        // ReSharper restore InconsistentNaming
#pragma warning disable SA1201
    }

    // ReSharper restore RedundantStringInterpolation
    // ReSharper restore UnusedMember.Local
}