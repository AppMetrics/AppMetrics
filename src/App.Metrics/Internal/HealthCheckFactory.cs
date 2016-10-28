// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.DependencyInjection;

namespace App.Metrics.Internal
{
    internal class HealthCheckFactory : IHealthCheckFactory
    {
        private readonly ConcurrentDictionary<string, HealthCheck> _checks =
            new ConcurrentDictionary<string, HealthCheck>();

        public HealthCheckFactory(IEnumerable<HealthCheck> healthChecks)
        {
            foreach (var check in healthChecks)
            {
                _checks.TryAdd(check.Name, check);
            }
        }

        public HealthCheckFactory()
        {
        }

        internal IReadOnlyDictionary<string, HealthCheck> Checks => _checks;

        public void Dispose()
        {
            //TODO: AH - implement dispose
        }

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
            _checks.TryAdd(healthCheck.Name, healthCheck);

            //_logger.HealthCheckRegistered(healthCheck.Name);
        }
    }
}