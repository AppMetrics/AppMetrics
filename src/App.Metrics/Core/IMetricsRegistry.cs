using System;
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

    public interface IMetricsRegistry
    {
        IRegistryDataProvider DataProvider { get; }

        void ClearAllMetrics();

        ICounter Counter<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : CounterImplementation;

        void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags);

        IHistogram Histogram<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : HistogramImplementation;

        IMeter Meter<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, MetricTags tags)
            where T : MeterImplementation;

        void ResetMetricsValues();

        ITimer Timer<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : TimerImplementation;
    }
}