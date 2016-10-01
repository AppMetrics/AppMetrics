using System;
using App.Metrics;
using App.Metrics.Utils;

namespace AspNet.Metrics
{
    public class AspNetMetricsContext
    {
        public AspNetMetricsContext(MetricsContext context, Func<HealthStatus> healthStatus, 
            Clock clock = null)
        {
            Context = context;
            HealthStatus = healthStatus;
            Clock = clock ?? Clock.Default;
        }

        public MetricsContext Context { get; }

        public Func<HealthStatus> HealthStatus { get; }

        public Clock Clock { get; }
    }
}