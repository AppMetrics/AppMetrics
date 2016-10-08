using System;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class NullMetricsContext : BaseMetricsContext
    {
        public NullMetricsContext(string context, IClock systemClock) 
            : base(context, new NullMetricsRegistry(), new DefaultMetricsBuilder(), new HealthChecks(), systemClock, () => systemClock.UtcDateTime)
        {
        }

        protected override IMetricsContext CreateChildContextInstance(string contextName)
        {
            throw new NotImplementedException();
        }
    }
}