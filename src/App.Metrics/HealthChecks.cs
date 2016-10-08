using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace App.Metrics
{
    /// <summary>
    ///     Structure describing the status of executing all the health checks operations.
    /// </summary>
    public struct HealthStatus
    {
        /// <summary>
        ///     Flag indicating whether any checks are registered
        /// </summary>
        public readonly bool HasRegisteredChecks;

        /// <summary>
        ///     All health checks passed.
        /// </summary>
        public readonly bool IsHealthy;

        /// <summary>
        ///     Result of each health check operation
        /// </summary>
        public readonly HealthCheck.Result[] Results;

        public HealthStatus(IEnumerable<HealthCheck.Result> results)
        {
            Results = results.ToArray();
            IsHealthy = Results.All(r => r.Check.IsHealthy);
            HasRegisteredChecks = Results.Length > 0;
        }
    }

    /// <summary>
    ///     Registry for health checks
    /// </summary>
    public static class HealthChecks
    {
        private static readonly ConcurrentDictionary<string, HealthCheck> checks = new ConcurrentDictionary<string, HealthCheck>();

        /// <summary>
        ///     Execute all registered checks and return overall.
        /// </summary>
        /// <returns>Status of the system.</returns>
        public static async Task<HealthStatus> GetStatus()
        {
            var results = await Task.WhenAll(checks.Values.OrderBy(v => v.Name).Select(v => v.ExecuteAsync()));
            return new HealthStatus(results);
        }
       

        /// <summary>
        ///     Registers an action to monitor. If the action throws the health check fails,
        ///     otherwise is successful and the returned string is used as status message.
        /// </summary>
        /// <param name="name">Name of the health check.</param>
        /// <param name="check">Function to execute.</param>
        public static void RegisterHealthCheck(string name, Func<Task<string>> check)
        {
            RegisterHealthCheck(new HealthCheck(name, check));
        }

        /// <summary>
        ///     Registers a function to monitor. If the function throws or returns an HealthCheckResult.Unhealthy the check fails,
        ///     otherwise the result of the function is used as a status.
        /// </summary>
        /// <param name="name">Name of the health check.</param>
        /// <param name="check">Function to execute</param>
        public static void RegisterHealthCheck(string name, Func<Task<HealthCheckResult>> check)
        {
            RegisterHealthCheck(new HealthCheck(name, check));
        }

        /// <summary>
        ///     Registers a custom health check.
        /// </summary>
        /// <param name="healthCheck">Custom health check to register.</param>
        public static void RegisterHealthCheck(HealthCheck healthCheck)
        {
            checks.TryAdd(healthCheck.Name, healthCheck);
        }

        /// <summary>
        ///     Remove all the registered health checks.
        /// </summary>
        public static void UnregisterAllHealthChecks()
        {
            checks.Clear();
        }
    }
}