// <copyright file="AppMetricsReportingLoggerExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Reporting;

// ReSharper disable CheckNamespace
namespace App.Metrics.Logging
// ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal static class AppMetricsReportingLoggerExtensions
    {
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        public static void ReportFailed(this ILog logger, IReportMetrics reporter, Exception ex)
        {
            logger.Error(ex, $"{reporter.GetType()} failed during execution");
        }

        public static void ReportFailed(this ILog logger, IReportMetrics reporter)
        {
            logger.Error($"{reporter.GetType()} failed during execution");
        }

        public static void ReportingCancelled(this ILog logger, OperationCanceledException ex)
        {
            logger.Error(ex, "Report execution cancelled");
        }

        public static void ReportingDisposedDuringExecution(this ILog logger, ObjectDisposedException ex)
        {
            logger.Error(ex, "Report execution stopped");
        }

        public static void ReportingFailedDuringExecution(this ILog logger, AggregateException ex)
        {
            logger.Error(ex.Flatten(), "Report execution stopped");
        }

        public static void ReportRan(this ILog logger, IReportMetrics reportMetricsReporter, long startTimestamp)
        {
            if (startTimestamp == 0)
            {
                return;
            }

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            logger.Debug("Report {ReportType} ran in {ElapsedMilliseconds}ms", reportMetricsReporter.GetType().FullName, elapsed.Milliseconds);
        }

        public static void ReportRunning(this ILog logger, IReportMetrics reportMetrics)
        {
            logger.Trace($"Running {reportMetrics.GetType()}");
        }
    }

    // ReSharper restore RedundantStringInterpolation
}