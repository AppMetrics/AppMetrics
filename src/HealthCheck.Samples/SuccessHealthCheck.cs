using App.Metrics;

namespace HealthCheck.Samples
{
    public class SuccessHealthCheck : App.Metrics.Core.HealthCheck
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