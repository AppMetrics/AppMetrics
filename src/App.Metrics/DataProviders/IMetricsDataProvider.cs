using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.DataProviders
{
    /// <summary>
    ///     A provider capable of returning the current values for a set of metrics
    /// </summary>
    public interface IMetricsDataProvider : IHideObjectMembers
    {
        /// <summary>
        ///     Returns the current metrics data for the context for which this provider has been created.
        /// </summary>
        MetricsData GetMetricsData(IMetricsContext metricsContext);
    }
}