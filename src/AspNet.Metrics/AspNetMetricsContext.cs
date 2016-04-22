using System;
using Metrics;

namespace AspNet.Metrics
{
    public class AspNetMetricsContext
    {
        public AspNetMetricsContext(MetricsContext context, Func<HealthStatus> healthStatus)
        {
            Context = context;
            HealthStatus = healthStatus;
        }

        public MetricsContext Context { get; }

        public Func<HealthStatus> HealthStatus { get; }
    }
}