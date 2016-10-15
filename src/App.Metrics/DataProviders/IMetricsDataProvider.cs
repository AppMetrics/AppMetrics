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
        MetricsData CurrentMetricsData { get; }
    }

    public static class FilteredMetricsExtensions
    {
        public static IMetricsDataProvider WithFilter(this IMetricsDataProvider provider, IMetricsFilter filter)
        {
            if (filter == null)
            {
                return provider;
            }
            return new FilteredMetricsDataProvider(provider, filter);
        }
    }
}