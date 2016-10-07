using System;
using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public sealed class DefaultRegistryDataProvider : IRegistryDataProvider
    {
        private readonly Func<IEnumerable<CounterValueSource>> _counters;
        private readonly Func<IEnumerable<GaugeValueSource>> _gauges;
        private readonly Func<IEnumerable<HistogramValueSource>> _histograms;
        private readonly Func<IEnumerable<MeterValueSource>> _meters;
        private readonly Func<IEnumerable<TimerValueSource>> _timers;

        public DefaultRegistryDataProvider(
            Func<IEnumerable<GaugeValueSource>> gauges,
            Func<IEnumerable<CounterValueSource>> counters,
            Func<IEnumerable<MeterValueSource>> meters,
            Func<IEnumerable<HistogramValueSource>> histograms,
            Func<IEnumerable<TimerValueSource>> timers)
        {
            _gauges = gauges;
            _counters = counters;
            _meters = meters;
            _histograms = histograms;
            _timers = timers;
        }

        public IEnumerable<CounterValueSource> Counters
        {
            get { return _counters(); }
        }

        public IEnumerable<GaugeValueSource> Gauges
        {
            get { return _gauges(); }
        }

        public IEnumerable<HistogramValueSource> Histograms
        {
            get { return _histograms(); }
        }

        public IEnumerable<MeterValueSource> Meters
        {
            get { return _meters(); }
        }

        public IEnumerable<TimerValueSource> Timers
        {
            get { return _timers(); }
        }
    }
}