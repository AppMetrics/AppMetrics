using System;
using App.Metrics.Core;
using App.Metrics.MetricData;

namespace App.Metrics.Internal
{
    internal interface IMetricsRegistry
    {
        bool AddGroup(string groupName, IMetricGroupRegistry registry);

        void Clear();

        ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterImplementation;

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        MetricsData GetData();

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramImplementation;

        IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterImplementation;

        void RemoveGroup(string groupName);

        ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerImplementation;
    }
}