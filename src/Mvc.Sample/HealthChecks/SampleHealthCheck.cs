using App.Metrics;

namespace Mvc.Sample.HealthChecks
{
    public class SampleHealthCheck : App.Metrics.Core.HealthCheck
    {
        public SampleHealthCheck() : base("Sample Healthy")
        {
        }

        protected override HealthCheckResult Check()
        {
            return HealthCheckResult.Healthy("OK");
        }
    }
}