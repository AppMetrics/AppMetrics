// <copyright file="DefaultReportGenerator.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.Internal
{
    internal class DefaultReportGenerator
    {
        private readonly ILogger _logger;

        public DefaultReportGenerator(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<DefaultReportGenerator>() ?? throw new ArgumentNullException(nameof(loggerFactory));
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
            var startTimestamp = _logger.IsEnabled(LogLevel.Trace) ? Stopwatch.GetTimestamp() : 0;

            _logger.ReportedStarted(reporter);

            if (reporterMetricsFilter == default(IFilterMetrics))
            {
                reporterMetricsFilter = metrics.GlobalFilter;
            }

            reporter.StartReportRun(metrics);

            var data = metrics.Snapshot.Get(reporterMetricsFilter);

            ReportMetricTypes(reporter, token, data);

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

        private static void ReportMetricTypes(IMetricReporter reporter, CancellationToken token, MetricsDataValueSource data)
        {
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
        }
    }
}