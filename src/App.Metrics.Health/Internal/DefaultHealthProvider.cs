// <copyright file="DefaultHealthProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Health.Internal
{
    internal sealed class DefaultHealthProvider : IProvideHealth
    {
        private readonly IHealthCheckRegistry _healthCheckRegistry;
        private readonly ILogger<DefaultHealthProvider> _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultHealthProvider" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="healthCheckRegistry">The health check registry.</param>
        public DefaultHealthProvider(ILogger<DefaultHealthProvider> logger, IHealthCheckRegistry healthCheckRegistry)
        {
            _logger = logger;
            _healthCheckRegistry = healthCheckRegistry;
        }

        /// <inheritdoc />
        public async ValueTask<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var startTimestamp = _logger.IsEnabled(LogLevel.Trace) ? Stopwatch.GetTimestamp() : 0;

            _logger.HealthCheckGetStatusExecuting();

            var results = await Task.WhenAll(
                _healthCheckRegistry.Checks.Values.OrderBy(v => v.Name).Select(v => v.ExecuteAsync(cancellationToken).AsTask()));

            var healthStatus = new HealthStatus(results);

            _logger.HealthCheckGetStatusExecuted(healthStatus, startTimestamp);

            return healthStatus;
        }
    }
}