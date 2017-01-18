// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultHealthManager : IHealthStatusProvider
    {
        private readonly IHealthCheckFactory _healthCheckFactory;
        private readonly ILogger<DefaultHealthManager> _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHealthManager" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="healthCheckFactory">The health check factory.</param>
        public DefaultHealthManager(ILogger<DefaultHealthManager> logger, IHealthCheckFactory healthCheckFactory)
        {
            _logger = logger;
            _healthCheckFactory = healthCheckFactory ?? new NoOpHealthCheckFactory();
        }

        /// <inheritdoc />
        public async Task<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(
                _healthCheckFactory.Checks.Values.OrderBy(v => v.Name)
                                   .Select(v => v.ExecuteAsync(cancellationToken)));

            var healthStatus = new HealthStatus(results.Where(h => !h.Check.Status.IsIgnored()));

            _logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}