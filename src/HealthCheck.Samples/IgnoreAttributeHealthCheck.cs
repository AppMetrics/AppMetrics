using System;
using App.Metrics;

namespace HealthCheck.Samples
{
    [Obsolete]
    public class IgnoreAttributeHealthCheck : App.Metrics.Core.HealthCheck
    {
        public IgnoreAttributeHealthCheck() : base("Referencing Assembly - Sample Healthy")
        {
        }

        protected override HealthCheckResult Check()
        {
            return HealthCheckResult.Healthy("OK");
        }
    }
}