using Metrics;

namespace HealthCheck.Samples
{
    public class SuccessHealthCheck : Metrics.Core.HealthCheck
    {
        public SuccessHealthCheck() : base("Referencing Assembly - Sample Healthy")
        {
        }

        protected override HealthCheckResult Check()
        {
            return HealthCheckResult.Healthy("OK");
        }
    }
}