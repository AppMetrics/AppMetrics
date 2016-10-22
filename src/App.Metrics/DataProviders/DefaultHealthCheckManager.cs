// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Registries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.DataProviders
{
    public sealed class DefaultHealthCheckManager : IHealthCheckManager
    {
        private readonly IHealthCheckRegistry _healthCheckRegistry;
        private readonly ILogger _logger;

        public DefaultHealthCheckManager(IOptions<AppMetricsOptions> options,
            ILoggerFactory loggerFactory,
            IHealthCheckRegistry healthCheckRegistry)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));    
            }

            if (healthCheckRegistry == null)
            {
                throw new ArgumentNullException(nameof(healthCheckRegistry));
            }

            _healthCheckRegistry = healthCheckRegistry;
            options.Value.HealthCheckRegistry(_healthCheckRegistry);
            _logger = loggerFactory.CreateLogger<DefaultHealthCheckManager>();
        }

        public async Task<HealthStatus> GetStatusAsync()
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(_healthCheckRegistry.Checks.Values.OrderBy(v => v.Name)
                .Select(v => v.ExecuteAsync()));

            var healthStatus = new HealthStatus(results);

            _logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}