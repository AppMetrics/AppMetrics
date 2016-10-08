using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace App.Metrics
{
    /// <summary>
    ///     Registry for health checks
    /// </summary>
    public class HealthChecks
    {
        private static readonly ConcurrentDictionary<string, HealthCheck> Checks = new ConcurrentDictionary<string, HealthCheck>();

        /// <summary>
        ///     Execute all registered checks and return overall.
        /// </summary>
        /// <returns>Status of the system.</returns>
        public async Task<HealthStatus> GetStatusAsync()
        {
            var results = await Task.WhenAll(Checks.Values.OrderBy(v => v.Name).Select(v => v.ExecuteAsync()));
            return new HealthStatus(results);
        }


        /// <summary>
        ///     Registers an action to monitor. If the action throws the health check fails,
        ///     otherwise is successful and the returned string is used as status message.
        /// </summary>
        /// <param name="name">Name of the health check.</param>
        /// <param name="check">Function to execute.</param>
        public void RegisterHealthCheck(string name, Func<Task<string>> check)
        {
            RegisterHealthCheck(new HealthCheck(name, check));
        }

        /// <summary>
        ///     Registers a function to monitor. If the function throws or returns an HealthCheckResult.Unhealthy the check fails,
        ///     otherwise the result of the function is used as a status.
        /// </summary>
        /// <param name="name">Name of the health check.</param>
        /// <param name="check">Function to execute</param>
        public void RegisterHealthCheck(string name, Func<Task<HealthCheckResult>> check)
        {
            RegisterHealthCheck(new HealthCheck(name, check));
        }

        /// <summary>
        ///     Registers a custom health check.
        /// </summary>
        /// <param name="healthCheck">Custom health check to register.</param>
        internal void RegisterHealthCheck(HealthCheck healthCheck)
        {
            Checks.TryAdd(healthCheck.Name, healthCheck);
        }

        /// <summary>
        ///     Remove all the registered health checks.
        /// </summary>
        public void UnregisterAllHealthChecks()
        {
            Checks.Clear();
        }
    }
}