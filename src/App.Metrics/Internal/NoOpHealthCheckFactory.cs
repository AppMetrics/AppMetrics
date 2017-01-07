using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace App.Metrics.Internal
{
    internal class NoOpHealthCheckFactory : IHealthCheckFactory
    {
        public ConcurrentDictionary<string, HealthCheck> Checks { get; } = new ConcurrentDictionary<string, HealthCheck>();

        public void Register(string name, Func<Task<string>> check)
        {
        }

        public void Register(string name, Func<Task<HealthCheckResult>> check)
        {
        }
    }
}