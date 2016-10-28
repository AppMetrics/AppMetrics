using System;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.MetricData;

namespace App.Metrics.Internal
{
    internal interface IMetricsRegistry
    {
        bool AddGroup(string groupName, IMetricGroupRegistry registry);

        void Clear();

        ICounter Counter<T>(CounterOptions options, Func<T> builder) where T : ICounterMetric;

        void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        Task<MetricsData> GetDataAsync();

        IHistogram Histogram<T>(HistogramOptions options, Func<T> builder) where T : IHistogramMetric;

        IMeter Meter<T>(MeterOptions options, Func<T> builder) where T : IMeterMetric;

        void RemoveGroup(string groupName);

        ITimer Timer<T>(TimerOptions options, Func<T> builder) where T : ITimerMetric;
    }
}