// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Registries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Internal
{
    internal sealed class DefaultHealthCheckRegistry : IHealthCheckRegistry
    {
        private readonly ILogger _logger;

        public DefaultHealthCheckRegistry(ILoggerFactory loggerFactory,
            IEnumerable<HealthCheck> healthChecks,
            IOptions<AppMetricsOptions> options)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _logger = loggerFactory.CreateLogger<DefaultHealthCheckRegistry>();

            Checks = new ConcurrentDictionary<string, HealthCheck>();

            if (options.Value.DisableHealthChecks) return;

            foreach (var healthCheck in healthChecks)
            {
                Register(healthCheck);
            }
        }

        public ConcurrentDictionary<string, HealthCheck> Checks { get; }

        public void Register(string name, Func<Task<string>> check)
        {
            Register(new HealthCheck(name, check));
        }

        public void Register(string name, Func<Task<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));
        }

        public void UnregisterAllHealthChecks()
        {
            Checks.Clear();

            _logger.HealthChecksUnRegistered();
        }

        internal void Register(HealthCheck healthCheck)
        {
            Checks.TryAdd(healthCheck.Name, healthCheck);

            _logger.HealthCheckRegistered(healthCheck.Name);
        }
    }
}