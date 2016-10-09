using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface ICounterImplementation : ICounter, IMetricValueProvider<CounterValue>
    {
    }
}