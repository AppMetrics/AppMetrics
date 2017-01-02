using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace Api.InfluxDB.Sample.HealthChecks
{
    public class SampleDegradedHealthCheck : HealthCheck
    {
        public SampleDegradedHealthCheck() : base("Sample Degraded")
        {
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Degraded("Degraded"));
        }
    }
}