using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace HealthCheck.Samples
{
    public class SuccessHealthCheck : App.Metrics.Core.HealthCheck
    {
        public SuccessHealthCheck() : base("Referencing Assembly - Sample Healthy")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}