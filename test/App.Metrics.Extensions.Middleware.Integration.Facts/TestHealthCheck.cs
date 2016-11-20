using System.Threading.Tasks;
using App.Metrics.Core;

namespace App.Metrics.Extensions.Middleware.Integration.Facts
{
    public class TestHealthCheck : HealthCheck
    {
        public TestHealthCheck() : base("Test Health Check")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}