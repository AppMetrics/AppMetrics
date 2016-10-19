using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Health;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Registries
{
    public class HealthCheckRegistry : IHealthCheckRegistry
    {
        private readonly ILogger _logger;

        public HealthCheckRegistry(ILoggerFactory loggerFactory,
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

            _logger = loggerFactory.CreateLogger<HealthCheckRegistry>();

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