// <copyright file="DefaultHealthProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Health.Internal
{
    internal sealed class DefaultHealthProvider : IProvideHealth
    {
        private readonly IHealthCheckFactory _healthCheckFactory;
        private readonly ILogger<DefaultHealthProvider> _logger;
        private readonly Lazy<IMetrics> _metrics;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHealthProvider" /> class.
        /// </summary>
        /// <param name="metrics">Metrics instance to record health results</param>
        /// <param name="logger">The logger.</param>
        /// <param name="healthCheckFactory">The health check factory.</param>
        public DefaultHealthProvider(Lazy<IMetrics> metrics, ILogger<DefaultHealthProvider> logger, IHealthCheckFactory healthCheckFactory)
        {
            _metrics = metrics;
            _logger = logger;
            _healthCheckFactory = healthCheckFactory ?? new NoOpHealthCheckFactory();
        }

        /// <inheritdoc />
        public async ValueTask<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Trace) ? Stopwatch.GetTimestamp() : 0;

            _logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(
                _healthCheckFactory.Checks.Values.OrderBy(v => v.Name).Select(v => v.ExecuteAsync(cancellationToken).AsTask()));

            var failed = new List<HealthCheck.Result>();
            var degraded = new List<HealthCheck.Result>();

            foreach (var result in results)
            {
                var tags = new MetricTags(HealthConstants.TagKeys.HealthCheckName, result.Name);

                if (result.Check.Status == HealthCheckStatus.Degraded)
                {
                    degraded.Add(result);
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.degraded);
                }
                else if (result.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    failed.Add(result);
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.unhealthy);
                }
                else if (result.Check.Status == HealthCheckStatus.Healthy)
                {
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, HealthConstants.HealthScore.healthy);
                }
            }

            var healthStatus = new HealthStatus(results);

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

            _logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}