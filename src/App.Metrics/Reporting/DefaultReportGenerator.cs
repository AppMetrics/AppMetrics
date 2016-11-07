// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Infrastructure;

namespace App.Metrics.Reporting
{
    internal class DefaultReportGenerator
    {
        public async Task Generate(IMetricReporter reporter, 
            IMetricsContext metricsContext, 
            IMetricsFilter filter, 
            MetricTags globalTags,
            CancellationToken token)
        {
            var reportEnvironment = true;
            var reportHealthChecks = true;

            reporter.StartReport(metricsContext);

            var data = await metricsContext.Advanced.DataManager.GetAsync();

            if (filter != default(IMetricsFilter))
            {
                data = data.Filter(filter);
                reportEnvironment = filter.ReportEnvironment;
                reportHealthChecks = filter.ReportHealthChecks;
            }

            if (data.Environment.Entries.Any() && reportEnvironment)
            {
                reporter.StartMetricTypeReport(typeof(EnvironmentInfo));

                reporter.ReportEnvironment(data.Environment);

                reporter.EndMetricTypeReport(typeof(EnvironmentInfo));
            }            

            foreach (var group in data.Groups)
            {
                ReportMetricType(reporter, group.Counters,
                    c => { reporter.ReportMetric($"{metricsContext.ContextName}.{@group.GroupName}", c, globalTags); }, token);

                ReportMetricType(reporter, group.Gauges,
                    g => { reporter.ReportMetric($"{metricsContext.ContextName}.{@group.GroupName}", g, globalTags); }, token);

                ReportMetricType(reporter, group.Histograms,
                    h => { reporter.ReportMetric($"{metricsContext.ContextName}.{@group.GroupName}", h, globalTags); }, token);

                ReportMetricType(reporter, group.Meters,
                    m => { reporter.ReportMetric($"{metricsContext.ContextName}.{@group.GroupName}", m, globalTags); }, token);

                ReportMetricType(reporter, group.Timers,
                    t => { reporter.ReportMetric($"{metricsContext.ContextName}.{@group.GroupName}", t, globalTags); }, token);
            }

            if (reportHealthChecks)
            {
                var healthStatus = await metricsContext.Advanced.HealthCheckManager.GetStatusAsync();

                reporter.StartMetricTypeReport(typeof(HealthStatus));

                var passed = healthStatus.Results.Where(r => r.Check.IsHealthy);
                var failed = healthStatus.Results.Where(r => !r.Check.IsHealthy);

                reporter.ReportHealth(passed, failed);

                reporter.EndMetricTypeReport(typeof(HealthStatus));
            }

            reporter.EndReport(metricsContext);
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