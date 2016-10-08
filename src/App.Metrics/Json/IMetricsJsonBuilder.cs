using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public interface IMetricsJsonBuilder
    {
        string MetricsMimeType { get; }

        string BuildJson(MetricsData data, EnvironmentInfo environmentInfo);

        string BuildJson(MetricsData data, EnvironmentInfo environmentInfo, IClock clock, bool indented = true);
    }
}