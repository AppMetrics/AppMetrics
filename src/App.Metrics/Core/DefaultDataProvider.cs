using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public class DefaultDataProvider : MetricsDataProvider
    {
        private readonly Func<IEnumerable<MetricsDataProvider>> childProviders;
        private readonly string context;
        private readonly IEnumerable<EnvironmentEntry> environment;
        private readonly RegistryDataProvider registryDataProvider;
        private readonly Func<DateTime> timestampProvider;

        public DefaultDataProvider(string context,
            Func<DateTime> timestampProvider,
            RegistryDataProvider registryDataProvider,
            Func<IEnumerable<MetricsDataProvider>> childProviders)
            : this(context, timestampProvider, Enumerable.Empty<EnvironmentEntry>(), registryDataProvider, childProviders)
        {
        }

        public DefaultDataProvider(string context,
            Func<DateTime> timestampProvider,
            IEnumerable<EnvironmentEntry> environment,
            RegistryDataProvider registryDataProvider,
            Func<IEnumerable<MetricsDataProvider>> childProviders)
        {
            this.context = context;
            this.timestampProvider = timestampProvider;
            this.environment = environment;
            this.registryDataProvider = registryDataProvider;
            this.childProviders = childProviders;
        }

        public MetricsData CurrentMetricsData
        {
            get
            {
                return new MetricsData(this.context, this.timestampProvider(),
                    this.environment,
                    this.registryDataProvider.Gauges.ToArray(),
                    this.registryDataProvider.Counters.ToArray(),
                    this.registryDataProvider.Meters.ToArray(),
                    this.registryDataProvider.Histograms.ToArray(),
                    this.registryDataProvider.Timers.ToArray(),
                    this.childProviders().Select(p => p.CurrentMetricsData));
            }
        }
    }
}