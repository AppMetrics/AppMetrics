// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal
{
    internal sealed class DefaultHealthCheckManager : IHealthCheckManager
    {
        private readonly ILogger _logger;
        private readonly Func<IReadOnlyDictionary<string, HealthCheck>> _healthChecks;

        public DefaultHealthCheckManager(ILoggerFactory loggerFactory,
            Func<IReadOnlyDictionary<string, HealthCheck>> healthChecks)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<DefaultHealthCheckManager>();
            _healthChecks = healthChecks;
        }

        public async Task<HealthStatus> GetStatusAsync()
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _logger.HealthCheckGetStatusExecuting();

            var checks = _healthChecks();

            var results = await Task.WhenAll(checks.Values.OrderBy(v => v.Name)
                .Select(v => v.ExecuteAsync()));

            var healthStatus = new HealthStatus(results);

            _logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}