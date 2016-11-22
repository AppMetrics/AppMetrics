using System;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace HealthCheck.Samples
{
    [Obsolete]
    public class IgnoreAttributeHealthCheck : App.Metrics.Core.HealthCheck
    {
        public IgnoreAttributeHealthCheck() : base("Referencing Assembly - Sample Ignore")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}