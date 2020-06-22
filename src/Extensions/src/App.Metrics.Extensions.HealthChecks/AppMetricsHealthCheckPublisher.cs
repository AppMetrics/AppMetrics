// <copyright file="AppMetricsHealthCheckPublisher.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace App.Metrics.Extensions.HealthChecks
{
    public class AppMetricsHealthCheckPublisher : IHealthCheckPublisher
    {
        private readonly IMetrics _metrics;

        public AppMetricsHealthCheckPublisher(
            IMetrics metrics)
        {
            _metrics = metrics;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            foreach (var item in report.Entries)
            {
                var tags = new MetricTags(HealthReportingConstants.TagKeys.HealthCheckName, item.Key);
                _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, ((float)item.Value.Status) / 2);
            }

            _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.HealthGauge, ((float)report.Status) / 2);

            return Task.CompletedTask;
        }
    }
}