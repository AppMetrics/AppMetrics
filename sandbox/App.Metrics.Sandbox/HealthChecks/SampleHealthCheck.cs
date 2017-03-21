using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Sandbox.HealthChecks
{
    public class SampleHealthCheck : HealthCheck
    {
        /// <inheritdoc />
        public SampleHealthCheck() : base("Random Health Check")
        {
        }

        /// <inheritdoc />
        protected override Task<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (DateTime.UtcNow.Second <= 20)
            {
                return Task.FromResult(HealthCheckResult.Degraded());
            }

            if (DateTime.UtcNow.Second >= 40)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy());                
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
