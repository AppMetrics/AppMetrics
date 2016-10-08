using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsContext : BaseMetricsContext
    {
        private readonly IClock _systemClock;

        public DefaultMetricsContext(IClock systemClock)
            : this(string.Empty, systemClock)
        {
        }

        public DefaultMetricsContext(string context, IClock systemClock)
            : base(context, new DefaultMetricsRegistry(), new DefaultMetricsBuilder(), new HealthChecks(), systemClock, () => systemClock.UtcDateTime)
        {
            _systemClock = systemClock;
        }

        protected override IMetricsContext CreateChildContextInstance(string contextName)
        {
            return new DefaultMetricsContext(contextName, SystemClock);
        }

        internal IMetricsContext Internal => new NullMetricsContext(InternalMetricsContextName, _systemClock);
    }
}