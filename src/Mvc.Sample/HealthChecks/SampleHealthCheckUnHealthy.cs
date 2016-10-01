using App.Metrics;

namespace Mvc.Sample.HealthChecks
{
    public class SampleHealthCheckUnHealthy : App.Metrics.Core.HealthCheck
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