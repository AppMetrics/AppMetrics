using App.Metrics;

namespace HealthCheck.Samples
{
    public class HealthCheckUnHealthy : App.Metrics.Core.HealthCheck
    {
        public HealthCheckUnHealthy() : base("Referencing Assembly - Sample UnHealthy")
        {
        }

        protected override HealthCheckResult Check()
        {
            return HealthCheckResult.Unhealthy("OOPS");
        }
    }
}