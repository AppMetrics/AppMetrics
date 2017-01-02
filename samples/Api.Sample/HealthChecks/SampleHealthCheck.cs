using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace Api.Sample.HealthChecks
{
    public class SampleHealthCheck : HealthCheck
    {
        public SampleHealthCheck() : base("Sample Healthy")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}