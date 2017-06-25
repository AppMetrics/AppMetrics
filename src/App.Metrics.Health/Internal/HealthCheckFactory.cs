// <copyright file="HealthCheckFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Health.Internal
{
    internal sealed class HealthCheckFactory : IHealthCheckFactory
    {
        private readonly ILogger<HealthCheckFactory> _logger;

        public HealthCheckFactory(
            ILogger<HealthCheckFactory> logger,
            Lazy<IMetrics> metrics,
            IEnumerable<HealthCheck> healthChecks)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));

            foreach (var check in healthChecks)
            {
                Register(check);
            }
        }

        public HealthCheckFactory(ILogger<HealthCheckFactory> logger, Lazy<IMetrics> metrics)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        }

        /// <inheritdoc />
        public ConcurrentDictionary<string, HealthCheck> Checks { get; } = new ConcurrentDictionary<string, HealthCheck>();

        /// <inheritdoc />
        public Lazy<IMetrics> Metrics { get; }

        /// <inheritdoc />
        public void Register(string name, Func<ValueTask<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));
        }

        /// <inheritdoc />
        public void Register(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));
        }

        internal void Register(HealthCheck healthCheck)
        {
            if (Checks.TryAdd(healthCheck.Name, healthCheck))
            {
                _logger.HealthCheckRegistered(healthCheck.Name);
            }
        }
    }
}