using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.DataProviders
{
    public class DefaultMetricsDataProvider : IMetricsDataProvider
    {
        private readonly IClock _clock;
        private readonly IEnumerable<EnvironmentInfoEntry> _environment;

        public DefaultMetricsDataProvider(
            IClock clock,
            IEnumerable<EnvironmentInfoEntry> environment)
        {
            _clock = clock;
            _environment = environment;
        }

        public MetricsData GetMetricsData(IMetricsContext metricsContext)
        {
            var registryDataProvider = metricsContext.Advanced.RegistryDataProvider;

            return new MetricsData(metricsContext.Name, _clock.UtcDateTime,
                _environment,
                registryDataProvider.Gauges.ToArray(),
                registryDataProvider.Counters.ToArray(),
                registryDataProvider.Meters.ToArray(),
                registryDataProvider.Histograms.ToArray(),
                registryDataProvider.Timers.ToArray(),
                metricsContext.Advanced.ChildContexts.Values.Select(p => p.Advanced.MetricsDataProvider.GetMetricsData(p)));
        }
    }
}