// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal
{
    internal class HealthCheckFactory : IHealthCheckFactory
    {
        private readonly ConcurrentDictionary<string, HealthCheck> _checks =
            new ConcurrentDictionary<string, HealthCheck>();

        private readonly ILogger<HealthCheckFactory> _logger;

        public HealthCheckFactory(ILogger<HealthCheckFactory> logger, IEnumerable<HealthCheck> healthChecks)
        {
            _logger = logger;

            foreach (var check in healthChecks)
            {
                Register(check);
            }
        }

        public HealthCheckFactory(ILogger<HealthCheckFactory> logger)
        {
            _logger = logger;
        }

        public IReadOnlyDictionary<string, HealthCheck> Checks => _checks;

        public void Register(string name, Func<Task<string>> check)
        {
            Register(new HealthCheck(name, check));
        }

        public void Register(string name, Func<Task<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));
        }

        internal void Register(HealthCheck healthCheck)
        {
            if (_checks.TryAdd(healthCheck.Name, healthCheck))
            {
                _logger.HealthCheckRegistered(healthCheck.Name);
            }
        }
    }
}