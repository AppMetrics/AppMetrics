using System.Threading.Tasks;
using App.Metrics;

namespace HealthCheck.Samples
{
    public class SuccessHealthCheck : App.Metrics.Core.HealthCheck
    {
        public SuccessHealthCheck() : base("Referencing Assembly - Sample Healthy")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}