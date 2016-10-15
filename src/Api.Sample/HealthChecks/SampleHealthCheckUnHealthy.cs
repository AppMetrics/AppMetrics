using App.Metrics;
using App.Metrics.Core;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace Api.Sample.HealthChecks
{
    public class SampleHealthCheckUnHealthy : HealthCheck
    {
        public SampleHealthCheckUnHealthy() : base("Sample UnHealthy")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("OOPS"));
        }
    }
}