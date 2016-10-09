using System;
using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface IMetricsRegistry
    {
        IRegistryDataProvider DataProvider { get; }

        void ClearAllMetrics();

        ICounter Counter<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : ICounterImplementation;

        void Gauge(string name, Func<IMetricValueProvider<double>> valueProvider, Unit unit, MetricTags tags);

        IHistogram Histogram<T>(string name, Func<T> builder, Unit unit, MetricTags tags)
            where T : IHistogramImplementation;

        IMeter Meter<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, MetricTags tags)
            where T : IMeterImplementation;

        void ResetMetricsValues();

        ITimer Timer<T>(string name, Func<T> builder, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
            where T : ITimerImplementation;
    }
}