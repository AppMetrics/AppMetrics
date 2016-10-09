using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface IRegistryDataProvider
    {
        IEnumerable<CounterValueSource> Counters { get; }

        IEnumerable<GaugeValueSource> Gauges { get; }

        IEnumerable<HistogramValueSource> Histograms { get; }

        IEnumerable<MeterValueSource> Meters { get; }

        IEnumerable<TimerValueSource> Timers { get; }
    }
}