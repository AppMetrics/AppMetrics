using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Health;
using Microsoft.Extensions.Options;

namespace App.Metrics.Registries
{
    public class HealthCheckRegistry : IHealthCheckRegistry
    {
        public HealthCheckRegistry(IEnumerable<HealthCheck> healthChecks,
            IOptions<AppMetricsOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Checks = new ConcurrentDictionary<string, HealthCheck>();

            if (!options.Value.DisableHealthChecks)
            {
                foreach (var healthCheck in healthChecks)
                {
                    Checks.TryAdd(healthCheck.Name, healthCheck);
                }
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
        }

        internal void Register(HealthCheck healthCheck)
        {
            Checks.TryAdd(healthCheck.Name, healthCheck);
        }
    }
}