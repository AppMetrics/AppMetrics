using App.Metrics.DataProviders;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsContext : BaseMetricsContext
    {
        private readonly IHealthCheckDataProvider _healthCheckDataProvider;
        private readonly IClock _systemClock;

        public DefaultMetricsContext(IClock systemClock, IHealthCheckDataProvider healthCheckDataProvider)
            : this(string.Empty, systemClock, healthCheckDataProvider)
        {
        }

        public DefaultMetricsContext(string context, IClock systemClock, IHealthCheckDataProvider healthCheckDataProvider)
            : base(
                context, new DefaultMetricsRegistry(), new DefaultMetricsBuilder(), healthCheckDataProvider, systemClock,
                () => systemClock.UtcDateTime)
        {
            _systemClock = systemClock;
            _healthCheckDataProvider = healthCheckDataProvider;
        }

        internal IMetricsContext Internal => new NullMetricsContext(InternalMetricsContextName, _systemClock);

        protected override IMetricsContext CreateChildContextInstance(string contextName)
        {
            return new DefaultMetricsContext(contextName, SystemClock, _healthCheckDataProvider);
        }
    }
}