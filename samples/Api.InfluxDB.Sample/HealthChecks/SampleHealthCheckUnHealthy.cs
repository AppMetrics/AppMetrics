using System.Threading.Tasks;
using App.Metrics.Core;

namespace Api.InfluxDB.Sample.HealthChecks
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