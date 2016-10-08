using System;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class NullMetricsContext : BaseMetricsContext
    {
        //TODO: AH - default clock ok?
        //TODO: AH - null builder?
        public NullMetricsContext(string context) 
            : base(context, new NullMetricsRegistry(), new DefaultMetricsBuilder(), Clock.Default, () => Clock.Default.UtcDateTime)
        {
        }

        protected override IMetricsContext CreateChildContextInstance(string contextName)
        {
            throw new NotImplementedException();
        }
    }
}