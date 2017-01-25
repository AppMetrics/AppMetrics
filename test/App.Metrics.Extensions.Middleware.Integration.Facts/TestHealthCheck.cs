using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Health;

namespace App.Metrics.Extensions.Middleware.Integration.Facts
{
    public class TestHealthCheck : HealthCheck
    {
        public TestHealthCheck() : base("Test Health Check")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}