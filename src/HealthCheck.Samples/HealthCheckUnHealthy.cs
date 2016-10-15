using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Health;

namespace HealthCheck.Samples
{
    public class HealthCheckUnHealthy : App.Metrics.Core.HealthCheck
    {
        public HealthCheckUnHealthy() : base("Referencing Assembly - Sample UnHealthy")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("OOPS"));
        }
    }
}