// <copyright file="HealthResultsAsMetricsReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;
#if !NETSTANDARD1_6
using App.Metrics.Health.Internal;
 #endif
using App.Metrics.Health.Reporting.Metrics.Internal;

namespace App.Metrics.Health.Reporting.Metrics
{
    public class HealthResultsAsMetricsReporter : IReportHealthStatus
    {
        private static readonly ILog Logger = LogProvider.For<HealthResultsAsMetricsReporter>();
        private readonly IMetrics _metrics;
        private readonly HealthAsMetricsOptions _healthAsMetricsOptions;

        public HealthResultsAsMetricsReporter(IMetrics metrics)
            : this(metrics, new HealthAsMetricsOptions())
        {
        }

        public HealthResultsAsMetricsReporter(IMetrics metrics, HealthAsMetricsOptions healthAsMetricsOptions)
        {
            _healthAsMetricsOptions = healthAsMetricsOptions ?? throw new ArgumentNullException(nameof(healthAsMetricsOptions));
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));

            ReportInterval = healthAsMetricsOptions.ReportInterval > TimeSpan.Zero
                ? healthAsMetricsOptions.ReportInterval
                : HealthConstants.Reporting.DefaultReportInterval;

            Logger.Trace($"Using Metrics Reporter {this}. ReportInterval: {ReportInterval}");
        }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; set; }

        /// <inheritdoc />
        public Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default)
        {
            if (!_healthAsMetricsOptions.Enabled || !options.Enabled)
            {
                Logger.Trace($"Health Status Reporter `{this}` disabled, not reporting.");

#if NETSTANDARD1_6
                return Task.CompletedTask;
#else
                return AppMetricsHealthTaskHelper.CompletedTask();
#endif
            }

            Logger.Trace($"Health Status Reporter `{this}` reporting health status.");

            foreach (var healthResult in status.Results)
            {
                var tags = new MetricTags(HealthReportingConstants.TagKeys.HealthCheckName, healthResult.Name);

                if (healthResult.Check.Status == HealthCheckStatus.Degraded)
                {
                    _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.degraded);
                }
                else if (healthResult.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.unhealthy);
                }
                else if (healthResult.Check.Status == HealthCheckStatus.Healthy)
                {
                    _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.healthy);
                }
            }

            var overallHealthStatus = HealthConstants.HealthScore.healthy;

            if (status.Status == HealthCheckStatus.Unhealthy)
            {
                overallHealthStatus = HealthConstants.HealthScore.unhealthy;
            }
            else if (status.Status == HealthCheckStatus.Degraded)
            {
                overallHealthStatus = HealthConstants.HealthScore.degraded;
            }

            _metrics.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.HealthGauge, overallHealthStatus);

            Logger.Trace($"Health Status Reporter `{this}` successfully reported health status.");

#if NETSTANDARD1_6
            return Task.CompletedTask;
#else
            return AppMetricsHealthTaskHelper.CompletedTask();
#endif
        }
    }
}