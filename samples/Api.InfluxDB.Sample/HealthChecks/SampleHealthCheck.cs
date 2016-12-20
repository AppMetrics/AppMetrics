using System.Threading.Tasks;
using App.Metrics.Core;

namespace Api.InfluxDB.Sample.HealthChecks
{
    public class SampleHealthCheck : HealthCheck
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