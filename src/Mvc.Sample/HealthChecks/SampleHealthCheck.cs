using Metrics;
using Metrics.Core;

namespace Mvc.Sample.HealthChecks
{
    public class SampleHealthCheck : Metrics.Core.HealthCheck
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