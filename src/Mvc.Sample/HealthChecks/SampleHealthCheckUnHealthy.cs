using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Health;

namespace Mvc.Sample.HealthChecks
{
    public class SampleHealthCheckUnHealthy : App.Metrics.Health.HealthCheck
    {
        public SampleHealthCheckUnHealthy() : base("Sample UnHealthy")
        {
        }

        protected override Task<HealthCheckResult >CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("OOPS"));
        }
    }
}