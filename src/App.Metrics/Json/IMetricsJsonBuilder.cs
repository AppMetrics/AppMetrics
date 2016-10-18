using App.Metrics.Infrastructure;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public interface IMetricsJsonBuilder
    {
        string MetricsMimeType { get; }

        string BuildJson(IMetricsContext metricsContext,
            EnvironmentInfo environmentInfo,
             IMetricsFilter filter);

        string BuildJson(IMetricsContext metricsContext, EnvironmentInfo environmentInfo,
            IMetricsFilter filter,
            bool indented);
    }
}