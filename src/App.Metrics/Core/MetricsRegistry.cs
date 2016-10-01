using System;
using System.Collections.Generic;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface RegistryDataProvider
    {
        IEnumerable<CounterValueSource> Counters { get; }

        IEnumerable<GaugeValueSource> Gauges { get; }

        IEnumerable<HistogramValueSource> Histograms { get; }

        IEnumerable<MeterValueSource> Meters { get; }

        IEnumerable<TimerValueSource> Timers { get; }
    }

    public interface MetricsRegistry
    {
        RegistryDataProvider DataProvider { get; }

        void ClearAllMetrics();

        Counter Counter<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : CounterImplementation;

        void Gauge(string name, Func<MetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags);

        Histogram Histogram<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : HistogramImplementation;

        Meter Meter<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, MetricTags tags)
            where T : MeterImplementation;

        void ResetMetricsValues();

        Timer Timer<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : TimerImplementation;
    }
}