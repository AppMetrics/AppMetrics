// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Diagnostics;
using App.Metrics;
using App.Metrics.Internal;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.Logging
// ReSharper restore CheckNamespace
{
    internal static class AppMetricsReportingLoggerExtensions
    {
        static AppMetricsReportingLoggerExtensions()
        {
            _runReportsExecuted = LoggerMessage.Define<double>(
                LogLevel.Information,
                eventId: AppMetricsEventIds.Reports.Schedule,
                formatString:
                $"Executed {nameof(IMetricGroupRegistry)} RunReports in {{elapsedMilliseconds}}ms");

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
        }

        public static void ReportedStarted<TReport>(this ILogger logger, long startTimestamp)
            where TReport : IMetricReporter
        {
            if (!logger.IsEnabled(LogLevel.Information)) return;
            if (startTimestamp == 0) return;

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            _reportStarted(logger, typeof(TReport).FullName, elapsed.TotalMilliseconds, null);
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

        public static void ReportStarting<TReport>(this ILogger logger)
            where TReport : IMetricReporter
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
        static readonly Action<ILogger, double, Exception> _runReportsExecuted;
        private static readonly Action<ILogger, string, double, Exception> _reportStarted;
        private static readonly Action<ILogger, string, double, Exception> _reportRan;
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
        // ReSharper restore InconsistentNaming
    }
}