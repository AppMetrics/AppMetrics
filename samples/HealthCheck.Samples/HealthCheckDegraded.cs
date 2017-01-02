using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace HealthCheck.Samples
{
    public class HealthCheckDegraded : App.Metrics.Core.HealthCheck
    {
        public HealthCheckDegraded() : base("Referencing Assembly - Sample Degraded")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Degraded());
        }
    }
}