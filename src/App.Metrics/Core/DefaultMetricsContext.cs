using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsContext : BaseMetricsContext
    {
        public DefaultMetricsContext()
            : this(string.Empty)
        {
        }

        public DefaultMetricsContext(string context)
            : base(context, new DefaultMetricsRegistry(), new DefaultMetricsBuilder(), () => Clock.Default.UTCDateTime)
        {
        }

        protected override MetricsContext CreateChildContextInstance(string contextName)
        {
            return new DefaultMetricsContext(contextName);
        }
    }
}