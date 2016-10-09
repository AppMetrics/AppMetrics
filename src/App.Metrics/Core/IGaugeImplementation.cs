using App.Metrics.MetricData;

namespace App.Metrics.Core
{
    public interface IGaugeImplementation : IMetricValueProvider<double>
    {
    }
}