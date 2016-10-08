using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsContext : BaseMetricsContext
    {
        public DefaultMetricsContext(IClock clock)
            : this(string.Empty, clock)
        {
        }

        public DefaultMetricsContext(string context, IClock systemClock)
            : base(context, new DefaultMetricsRegistry(), new DefaultMetricsBuilder(), new HealthChecks(), systemClock, () => systemClock.UtcDateTime)
        {
        }

        protected override IMetricsContext CreateChildContextInstance(string contextName)
        {
            return new DefaultMetricsContext(contextName, SystemClock);
        }

        internal IMetricsContext Internal { get; } = new NullMetricsContext(InternalMetricsContextName);
    }
}