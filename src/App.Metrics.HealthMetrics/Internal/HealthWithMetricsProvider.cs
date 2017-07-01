// <copyright file="HealthWithMetricsProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Internal;

namespace App.Metrics.HealthMetrics.Internal
{
    internal sealed class HealthWithMetricsProvider : IProvideHealth
    {
        private readonly DefaultHealthProvider _defaultHealthProvider;
        private readonly Lazy<IMetrics> _metrics;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthWithMetricsProvider" /> class.
        /// </summary>
        /// <param name="metrics">Lazy instance of IMetrics to record health check results</param>
        /// <param name="defaultHealthProvider">The default health provider which executes all registered checks.</param>
        public HealthWithMetricsProvider(Lazy<IMetrics> metrics, DefaultHealthProvider defaultHealthProvider)
        {
            _metrics = metrics;
            _defaultHealthProvider = defaultHealthProvider;
        }

        /// <inheritdoc />
        public async ValueTask<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var healthStatus = await _defaultHealthProvider.ReadStatusAsync(cancellationToken);

            foreach (var result in healthStatus.Results)
            {
                var tags = new MetricTags(HealthConstants.TagKeys.HealthCheckName, result.Name);

                if (result.Check.Status == HealthCheckStatus.Degraded)
                {
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.degraded);
                }
                else if (result.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.unhealthy);
                }
                else if (result.Check.Status == HealthCheckStatus.Healthy)
                {
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.healthy);
                }
            }

            var overallHealthStatus = HealthConstants.HealthScore.healthy;

            if (healthStatus.Status == HealthCheckStatus.Unhealthy)
            {
                overallHealthStatus = HealthConstants.HealthScore.unhealthy;
            }
            else if (healthStatus.Status == HealthCheckStatus.Degraded)
            {
                overallHealthStatus = HealthConstants.HealthScore.degraded;
            }

            _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.HealthGauge, overallHealthStatus);

            return healthStatus;
        }
    }
}
