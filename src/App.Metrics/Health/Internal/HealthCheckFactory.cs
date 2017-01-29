// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Health.Abstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Health.Internal
{
    internal sealed class HealthCheckFactory : IHealthCheckFactory
    {
        private readonly ILogger<HealthCheckFactory> _logger;

        public HealthCheckFactory(ILogger<HealthCheckFactory> logger, IEnumerable<HealthCheck> healthChecks)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;

            foreach (var check in healthChecks)
            {
                Register(check);
            }
        }

        public HealthCheckFactory(ILogger<HealthCheckFactory> logger) { _logger = logger; }

        public ConcurrentDictionary<string, HealthCheck> Checks { get; } = new ConcurrentDictionary<string, HealthCheck>();

        public void Register(string name, Func<Task<string>> check) { Register(new HealthCheck(name, check)); }

        public void Register(string name, Func<Task<HealthCheckResult>> check) { Register(new HealthCheck(name, check)); }

        internal void Register(HealthCheck healthCheck)
        {
            if (Checks.TryAdd(healthCheck.Name, healthCheck))
            {
                _logger.HealthCheckRegistered(healthCheck.Name);
            }
        }
    }
}