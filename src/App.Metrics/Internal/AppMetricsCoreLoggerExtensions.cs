// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using App.Metrics.Health;
using App.Metrics.Registries;
using App.Metrics.Reporters;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.Logging
// ReSharper restore CheckNamespace
{
    internal static class AppMetricsCoreLoggerExtensions
    {
        static AppMetricsCoreLoggerExtensions()
        {
            _healthCheckRegistered = LoggerMessage.Define<string>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.HealthChecks.Registration,
                formatString: $"Health Check Registered: {{name}}");

            _healthGetStatusExecuted = LoggerMessage.Define<double, int>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.HealthChecks.Status,
                formatString:
                $"Executed {nameof(HealthStatus)}, in {{elapsedMilliseconds}}ms, IsHealthy: True. {{checksPassed}} health check results passed.");

            _healthGetStatusExecutedFailed = LoggerMessage.Define<double, int, int, IEnumerable<string>>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.HealthChecks.Status,
                formatString:
                $"Executed {nameof(HealthStatus)}, in {{elapsedMilliseconds}}ms, IsHealthy: False. {{checksPassed}} health check results passed. {{checksFailed}} health check results failed. Failed Checks: {{failedChecks}}");

            _runReportsExecuted = LoggerMessage.Define<double>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.Reports.Schedule,
                formatString:
                $"Executed {nameof(IMetricsRegistry)} RunReports in {{elapsedMilliseconds}}ms");

            _reportStarted = LoggerMessage.Define<string, double>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.Reports.Schedule,
                formatString:
                $"Report {{reportType}} started in {{elapsedMilliseconds}}ms");

            _reportRan = LoggerMessage.Define<string, double>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.Reports.Schedule,
                formatString:
                $"Report {{reportType}} ran in {{elapsedMilliseconds}}ms");

            _healthGetStatusExecutedNoResults = LoggerMessage.Define(
                LogLevel.Information,
                eventId: AppMetricsEventIds.HealthChecks.Status,
                formatString: $"Executed {nameof(HealthStatus)}, 0 health check results.");
        }

        public static void HealthCheckGetStatusExecuted(this ILogger logger, HealthStatus healthStatus,
            long startTimestamp)
        {
            if (!logger.IsEnabled(LogLevel.Information)) return;
            if (startTimestamp == 0) return;

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            if (healthStatus.HasRegisteredChecks)
            {
                if (healthStatus.IsHealthy)
                {
                    _healthGetStatusExecuted(logger, elapsed.TotalMilliseconds, healthStatus.Results.Length, null);
                    return;
                }

                var checksFailed = healthStatus.Results.Count(x => !x.Check.IsHealthy);
                var checksPassed = healthStatus.Results.Count(x => x.Check.IsHealthy);
                var failedChecks = healthStatus.Results.Where(h => h.Check.IsHealthy).Select(h => h.Name);

                _healthGetStatusExecutedFailed(logger, elapsed.TotalMilliseconds, checksFailed, checksPassed, failedChecks, null);
            }

            _healthGetStatusExecutedNoResults(logger, null);
        }

        public static void HealthCheckGetStatusExecuting(this ILogger logger)
        {
            logger.LogDebug(AppMetricsEventIds.HealthChecks.Status, "Executing HealthCheck Get Status");
        }

        public static void HealthCheckRegistered(this ILogger logger, string name)
        {
            _healthCheckRegistered(logger, name, null);
        }

        public static void HealthChecksUnRegistered(this ILogger logger)
        {
            logger.LogDebug("Health Checks Un-Registered");
        }

        public static void MetricsDataGetExecuted(this ILogger logger)
        {
            logger.LogDebug(AppMetricsEventIds.Metrics.Data, "Executed GetMetricsData");
        }

        public static void MetricsDataGetExecuting(this ILogger logger)
        {
            logger.LogDebug(AppMetricsEventIds.Metrics.Data, "Executing GetMetricsData");
        }

        public static void ReportedStarted<TReport>(this ILogger logger, long startTimestamp)
            where TReport : IMetricsReport
        {
            if (!logger.IsEnabled(LogLevel.Information)) return;
            if (startTimestamp == 0) return;

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            _reportStarted(logger, typeof(TReport).FullName, elapsed.TotalMilliseconds, null);
        }

        public static void ReportRan(this ILogger logger, IMetricsReport report, long startTimestamp)
        {
            if (!logger.IsEnabled(LogLevel.Information)) return;
            if (startTimestamp == 0) return;

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            _reportRan(logger, report.GetType().FullName, elapsed.TotalMilliseconds, null);
        }

        public static void ReportRunning(this ILogger logger, IMetricsReport report)
        {
            logger.LogInformation(AppMetricsEventIds.Reports.Schedule, $"Running {report.GetType()}");
        }

        public static void ReportStarting<TReport>(this ILogger logger)
            where TReport : IMetricsReport
        {
            logger.LogInformation(AppMetricsEventIds.Reports.Schedule, $"Starting {typeof(TReport)}");
        }

        public static void RunReportsExecuted(this ILogger logger, long startTimestamp)
        {
            if (!logger.IsEnabled(LogLevel.Information)) return;
            if (startTimestamp == 0) return;

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            _runReportsExecuted(logger, elapsed.TotalMilliseconds, null);
        }

        public static void RunReportsExecuting(this ILogger logger)
        {
            logger.LogInformation(AppMetricsEventIds.Reports.Schedule, "Executing Run Reports");
        }

        internal static class AppMetricsEventIds
        {
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

            private const int MetricsStart = 9999;
        }


        // ReSharper disable InconsistentNaming
        private static readonly Action<ILogger, string, Exception> _healthCheckRegistered;
        private static readonly Action<ILogger, double, int, Exception> _healthGetStatusExecuted;
        private static readonly Action<ILogger, double, int, int, IEnumerable<string>, Exception> _healthGetStatusExecutedFailed;
        private static readonly Action<ILogger, double, Exception> _runReportsExecuted;
        private static readonly Action<ILogger, string, double, Exception> _reportStarted;
        private static readonly Action<ILogger, string, double, Exception> _reportRan;
        private static readonly Action<ILogger, Exception> _healthGetStatusExecutedNoResults;
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
        // ReSharper restore InconsistentNaming
    }
}