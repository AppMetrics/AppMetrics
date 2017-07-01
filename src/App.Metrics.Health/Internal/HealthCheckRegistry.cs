// <copyright file="HealthCheckRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Internal
{
    internal sealed class HealthCheckRegistry : IHealthCheckRegistry
    {
        public HealthCheckRegistry(IEnumerable<HealthCheck> healthChecks)
        {
            Register(healthChecks);
        }

        public HealthCheckRegistry()
        {
        }

        /// <inheritdoc />
        public Dictionary<string, HealthCheck> Checks { get; } = new Dictionary<string, HealthCheck>();

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

        internal void Register(IEnumerable<HealthCheck> healthChecks)
        {
            foreach (var check in healthChecks)
            {
                Checks.Add(check.Name, check);
            }
        }

        internal void Register(HealthCheck healthCheck)
        {
            Checks.Add(healthCheck.Name, healthCheck);
        }
    }
}