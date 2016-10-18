using App.Metrics.MetricData;

namespace App.Metrics.DataProviders
{
    public sealed class NullMetricsDataProvider : IMetricsDataProvider
    {
        public MetricsData GetMetricsData(IMetricsContext metricsContext)
        {
            return MetricsData.Empty;
        }
    }
}