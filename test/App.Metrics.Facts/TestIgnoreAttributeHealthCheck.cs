using System;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Facts
{
    [Obsolete]
    public class IgnoreAttributeHealthCheck : App.Metrics.Core.HealthCheck
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