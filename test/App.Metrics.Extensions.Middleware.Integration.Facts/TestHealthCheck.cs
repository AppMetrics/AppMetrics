using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Extensions.Middleware.Integration.Facts
{
    // ReSharper disable UnusedMember.Global this is automatically registered
    public class TestHealthCheck : HealthCheck
        // ReSharper restore UnusedMember.Global
    {
        public TestHealthCheck() : base("Test Health Check") { }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}