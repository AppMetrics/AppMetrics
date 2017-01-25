// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filtering.Interfaces;
using App.Metrics.Health;
using App.Metrics.Reporting.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.Internal
{
    public class DefaultReportGenerator
    {
        private readonly ILogger _logger;

        public DefaultReportGenerator(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<DefaultReportGenerator>();
        }

        public Task<bool> GenerateAsync(
            IMetricReporter reporter,
            IMetrics metrics,
            CancellationToken token) { return GenerateAsync(reporter, metrics, metrics.GlobalFilter, token); }

        public async Task<bool> GenerateAsync(
            IMetricReporter reporter,
            IMetrics metrics,
            IFilterMetrics reporterMetricsFilter,
            CancellationToken token)
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _logger.ReportedStarted(reporter);

            if (reporterMetricsFilter == default(IFilterMetrics))
            {
                reporterMetricsFilter = metrics.GlobalFilter;
            }

            reporter.StartReportRun(metrics);

            var data = metrics.Snapshot.Get(reporterMetricsFilter);

            if (data.Environment.Entries.Any() && reporterMetricsFilter.ReportEnvironment)
            {
                reporter.ReportEnvironment(data.Environment);
            }

            if (reporterMetricsFilter.ReportHealthChecks)
            {
                var healthStatus = await metrics.Health.ReadStatusAsync(token);

                var passed = healthStatus.Results.Where(r => r.Check.Status.IsHealthy()).ToArray();
                var failed = healthStatus.Results.Where(r => r.Check.Status.IsUnhealthy()).ToArray();
                var degraded = healthStatus.Results.Where(r => r.Check.Status.IsDegraded()).ToArray();

                reporter.ReportHealth(metrics.GlobalTags, passed, degraded, failed);

                foreach (var check in passed)
                {
                    metrics.Measure.Counter.Increment(ApplicationHealthMetricRegistry.HealthyCheckCounter, check.Name);
                }

                foreach (var check in degraded)
                {
                    metrics.Measure.Counter.Increment(ApplicationHealthMetricRegistry.DegradedCheckCounter, check.Name);
                }

                foreach (var check in failed)
                {
                    metrics.Measure.Counter.Increment(ApplicationHealthMetricRegistry.UnhealthyCheckCounter, check.Name);
                }
            }

            foreach (var contextValueSource in data.Contexts)
            {
                ReportMetricType(
                    contextValueSource.Counters,
                    c => { reporter.ReportMetric($"{contextValueSource.Context}", c); },
                    token);

                ReportMetricType(
                    contextValueSource.Gauges,
                    g => { reporter.ReportMetric($"{contextValueSource.Context}", g); },
                    token);

                ReportMetricType(
                    contextValueSource.Histograms,
                    h => { reporter.ReportMetric($"{contextValueSource.Context}", h); },
                    token);

                ReportMetricType(
                    contextValueSource.Meters,
                    m => { reporter.ReportMetric($"{contextValueSource.Context}", m); },
                    token);

                ReportMetricType(
                    contextValueSource.Timers,
                    t => { reporter.ReportMetric($"{contextValueSource.Context}", t); },
                    token);

                ReportMetricType(
                    contextValueSource.ApdexScores,
                    t => { reporter.ReportMetric($"{contextValueSource.Context}", t); },
                    token);
            }

            var result = await reporter.EndAndFlushReportRunAsync(metrics);

            _logger.ReportRan(reporter, startTimestamp);

            return result;
        }

        private static void ReportMetricType<T>(IEnumerable<T> metrics, Action<T> report, CancellationToken token)
        {
            var reportingMetrics = metrics.ToList();

            if (token.IsCancellationRequested || !reportingMetrics.Any())
            {
                return;
            }

            foreach (var metric in reportingMetrics)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                report(metric);
            }
        }
    }
}