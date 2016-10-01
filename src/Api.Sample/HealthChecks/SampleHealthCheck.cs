using App.Metrics;
using App.Metrics.Core;

namespace Api.Sample.HealthChecks
{
    public class SampleHealthCheck : HealthCheck
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