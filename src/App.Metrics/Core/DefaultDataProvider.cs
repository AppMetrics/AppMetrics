using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public class DefaultDataProvider : MetricsDataProvider
    {
        private readonly Func<IEnumerable<MetricsDataProvider>> _childProviders;
        private readonly string _context;
        private readonly IEnumerable<EnvironmentEntry> _environment;
        private readonly RegistryDataProvider _registryDataProvider;
        private readonly Func<DateTime> _timestampProvider;

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
            _context = context;
            _timestampProvider = timestampProvider;
            _environment = environment;
            _registryDataProvider = registryDataProvider;
            _childProviders = childProviders;
        }

        public MetricsData CurrentMetricsData
        {
            get
            {
                return new MetricsData(_context, _timestampProvider(),
                    _environment,
                    _registryDataProvider.Gauges.ToArray(),
                    _registryDataProvider.Counters.ToArray(),
                    _registryDataProvider.Meters.ToArray(),
                    _registryDataProvider.Histograms.ToArray(),
                    _registryDataProvider.Timers.ToArray(),
                    _childProviders().Select(p => p.CurrentMetricsData));
            }
        }
    }
}