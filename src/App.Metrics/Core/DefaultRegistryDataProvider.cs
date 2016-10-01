using System;
using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public sealed class DefaultRegistryDataProvider : RegistryDataProvider
    {
        private readonly Func<IEnumerable<CounterValueSource>> counters;
        private readonly Func<IEnumerable<GaugeValueSource>> gauges;
        private readonly Func<IEnumerable<HistogramValueSource>> histograms;
        private readonly Func<IEnumerable<MeterValueSource>> meters;
        private readonly Func<IEnumerable<TimerValueSource>> timers;

        public DefaultRegistryDataProvider(
            Func<IEnumerable<GaugeValueSource>> gauges,
            Func<IEnumerable<CounterValueSource>> counters,
            Func<IEnumerable<MeterValueSource>> meters,
            Func<IEnumerable<HistogramValueSource>> histograms,
            Func<IEnumerable<TimerValueSource>> timers)
        {
            this.gauges = gauges;
            this.counters = counters;
            this.meters = meters;
            this.histograms = histograms;
            this.timers = timers;
        }

        public IEnumerable<CounterValueSource> Counters
        {
            get { return this.counters(); }
        }

        public IEnumerable<GaugeValueSource> Gauges
        {
            get { return this.gauges(); }
        }

        public IEnumerable<HistogramValueSource> Histograms
        {
            get { return this.histograms(); }
        }

        public IEnumerable<MeterValueSource> Meters
        {
            get { return this.meters(); }
        }

        public IEnumerable<TimerValueSource> Timers
        {
            get { return this.timers(); }
        }
    }
}