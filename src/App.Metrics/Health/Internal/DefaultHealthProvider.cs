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
using App.Metrics.Health.Abstractions;
using App.Metrics.Tagging;
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
        public async Task<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Trace) ? Stopwatch.GetTimestamp() : 0;

            _logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(
                _healthCheckFactory.Checks.Values.OrderBy(v => v.Name)
                                   .Select(v => v.ExecuteAsync(cancellationToken)));

            var failed = new List<HealthCheck.Result>();
            var degraded = new List<HealthCheck.Result>();

            foreach (var result in results)
            {
                var tags = new MetricTags(Constants.Health.TagKeys.HealthCheckName, result.Name);

                if (result.Check.Status == HealthCheckStatus.Degraded)
                {
                    degraded.Add(result);
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, Constants.Health.HealthScore.degraded);
                }
                else if (result.Check.Status == HealthCheckStatus.Unhealthy)
                {
                    failed.Add(result);
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, Constants.Health.HealthScore.unhealthy);
                }
                else if (result.Check.Status == HealthCheckStatus.Healthy)
                {
                    _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.Checks, tags, Constants.Health.HealthScore.healthy);
                }
            }

            var healthStatus = new HealthStatus(results.Where(h => !h.Check.Status.IsIgnored()));

            var overallHealthStatus = Constants.Health.HealthScore.healthy;

            if (healthStatus.Status == HealthCheckStatus.Unhealthy)
            {
                overallHealthStatus = Constants.Health.HealthScore.unhealthy;
            }
            else if (healthStatus.Status == HealthCheckStatus.Degraded)
            {
                overallHealthStatus = Constants.Health.HealthScore.degraded;
            }

            _metrics.Value.Measure.Gauge.SetValue(ApplicationHealthMetricRegistry.HealthGauge, overallHealthStatus);

            _logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}