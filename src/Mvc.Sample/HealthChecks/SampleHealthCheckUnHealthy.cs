using Metrics;
using Metrics.Core;

namespace Mvc.Sample.HealthChecks
{
    public class SampleHealthCheckUnHealthy : Metrics.Core.HealthCheck
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