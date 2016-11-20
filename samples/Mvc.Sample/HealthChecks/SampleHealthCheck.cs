using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Core;

namespace Mvc.Sample.HealthChecks
{
    public class SampleHealthCheck : App.Metrics.Core.HealthCheck
    {
        public SampleHealthCheck() : base("Sample Healthy")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}