// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Reporting.Internal
{
    internal class DefaultReportGenerator
    {
        internal Task Generate(IMetricReporter reporter,
            IMetrics metrics,
            CancellationToken token)
        {
            return Generate(reporter, metrics, metrics.Advanced.GlobalFilter, token);
        }

        internal async Task Generate(IMetricReporter reporter,
            IMetrics metrics,
            IMetricsFilter reporterMetricsFilter,
            CancellationToken token)
        {
            if (reporterMetricsFilter == default(IMetricsFilter))
            {
                reporterMetricsFilter = metrics.Advanced.GlobalFilter;
            }

            reporter.StartReport(metrics);

            var data = await metrics.Advanced.Data.ReadDataAsync(reporterMetricsFilter);

            if (data.Environment.Entries.Any() && reporterMetricsFilter.ReportEnvironment)
            {
                reporter.StartMetricTypeReport(typeof(EnvironmentInfo));

                reporter.ReportEnvironment(data.Environment);

                reporter.EndMetricTypeReport(typeof(EnvironmentInfo));
            }

            foreach (var contextValueSource in data.Contexts)
            {
                ReportMetricType(reporter, contextValueSource.Counters,
                    c => { reporter.ReportMetric($"{contextValueSource.Context}", c); }, token);

                ReportMetricType(reporter, contextValueSource.Gauges,
                    g => { reporter.ReportMetric($"{contextValueSource.Context}", g); }, token);

                ReportMetricType(reporter, contextValueSource.Histograms,
                    h => { reporter.ReportMetric($"{contextValueSource.Context}", h); }, token);

                ReportMetricType(reporter, contextValueSource.Meters,
                    m => { reporter.ReportMetric($"{contextValueSource.Context}", m); }, token);

                ReportMetricType(reporter, contextValueSource.Timers,
                    t => { reporter.ReportMetric($"{contextValueSource.Context}", t); }, token);
            }

            if (reporterMetricsFilter.ReportHealthChecks)
            {
                var healthStatus = await metrics.Advanced.Health.ReadStatusAsync();

                reporter.StartMetricTypeReport(typeof(HealthStatus));

                var passed = healthStatus.Results.Where(r => r.Check.Status.IsHealthy());
                var failed = healthStatus.Results.Where(r => r.Check.Status.IsUnhealthy());
                var degraded = healthStatus.Results.Where(r => r.Check.Status.IsDegraded());

                reporter.ReportHealth(passed, degraded, failed);

                reporter.EndMetricTypeReport(typeof(HealthStatus));
            }

            reporter.EndReport(metrics);
        }

        private static void ReportMetricType<T>(IMetricReporter reporter, IEnumerable<T> metrics, Action<T> report, CancellationToken token)
        {
            var reportingMetrics = metrics.ToList();

            if (token.IsCancellationRequested || !reportingMetrics.Any())
            {
                return;
            }

            reporter.StartMetricTypeReport(typeof(T));

            foreach (var metric in reportingMetrics)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                report(metric);
            }

            reporter.EndMetricTypeReport(typeof(T));
        }
    }
}