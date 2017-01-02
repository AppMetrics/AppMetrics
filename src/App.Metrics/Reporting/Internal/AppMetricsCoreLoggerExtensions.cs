// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Diagnostics;
using App.Metrics.Reporting.Interfaces;
using App.Metrics.Reporting.Internal;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.Logging
// ReSharper restore CheckNamespace
{
    internal static class AppMetricsReportingLoggerExtensions
    {
        static AppMetricsReportingLoggerExtensions()
        {
            _reportStarted = LoggerMessage.Define<string>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.Reports.Schedule,
                formatString:
                $"Report {{reportType}} started");

            _reportRan = LoggerMessage.Define<string, double>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.Reports.Schedule,
                formatString:
                $"Report {{reportType}} ran in {{elapsedMilliseconds}}ms");
        }

        public static void ReportedStarted(this ILogger logger, IMetricReporter reporter)
        {
            if (!logger.IsEnabled(LogLevel.Information)) return;

            _reportStarted(logger, reporter.GetType().FullName, null);
        }

        public static void ReportRan(this ILogger logger, IMetricReporter reporter, long startTimestamp)
        {
            if (!logger.IsEnabled(LogLevel.Information)) return;
            if (startTimestamp == 0) return;

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            _reportRan(logger, reporter.GetType().FullName, elapsed.TotalMilliseconds, null);
        }

        public static void ReportRunning(this ILogger logger, IMetricReporter reporter)
        {
            logger.LogInformation(AppMetricsEventIds.Reports.Schedule, $"Running {reporter.GetType()}");
        }

        public static void ReportFailed(this ILogger logger, IMetricReporter reporter, Exception ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex, $"{reporter.GetType()} failed during execution");
        }

        public static void ReportFailed(this ILogger logger, IMetricReporter reporter)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, $"{reporter.GetType()} failed during execution");
        }

        public static void ReportingCancelled(this ILogger logger, OperationCanceledException ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex, "Report execution cancelled");
        }

        public static void ReportingFailedDuringExecution(this ILogger logger, AggregateException ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex.Flatten(), "Report execution stopped");
        }

        public static void ReportingDisposedDuringExecution(this ILogger logger, ObjectDisposedException ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex, "Report execution stopped");
        }

        internal static class AppMetricsEventIds
        {
            private const int MetricsStart = 9999;

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


        // ReSharper disable InconsistentNaming
        private static readonly Action<ILogger, string, Exception> _reportStarted;
        private static readonly Action<ILogger, string, double, Exception> _reportRan;
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
        // ReSharper restore InconsistentNaming
    }
}