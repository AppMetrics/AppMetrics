using System;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics
{
    public interface IHealthCheckFactory : IDisposable
    {
        void Register(string name, Func<Task<string>> check);

        void Register(string name, Func<Task<HealthCheckResult>> check);
    }
}