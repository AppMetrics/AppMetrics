using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Health;

namespace HealthCheck.Samples
{
    [Obsolete]
    public class IgnoreAttributeHealthCheck : App.Metrics.Health.HealthCheck
    {
        public IgnoreAttributeHealthCheck() : base("Referencing Assembly - Sample Healthy")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}