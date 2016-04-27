using Metrics;
using Metrics.Core;

namespace Api.Sample.HealthChecks
{
    public class SampleHealthCheckUnHealthy : HealthCheck
    {
        public SampleHealthCheckUnHealthy() : base("Sample UnHealthy")
        {
        }

        protected override HealthCheckResult Check()
        {
            return HealthCheckResult.Unhealthy("OOPS");
        }
    }
}