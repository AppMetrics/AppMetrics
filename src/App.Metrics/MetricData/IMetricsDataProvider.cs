using App.Metrics.Utils;

namespace App.Metrics.MetricData
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

    public sealed class FilteredMetrics : IMetricsDataProvider
    {
        private readonly IMetricsFilter _filter;
        private readonly IMetricsDataProvider _provider;

        public FilteredMetrics(IMetricsDataProvider provider, IMetricsFilter filter)
        {
            this._provider = provider;
            this._filter = filter;
        }

        public MetricsData CurrentMetricsData
        {
            get { return this._provider.CurrentMetricsData.Filter(this._filter); }
        }
    }

    public static class FilteredMetricsExtensions
    {
        public static IMetricsDataProvider WithFilter(this IMetricsDataProvider provider, IMetricsFilter filter)
        {
            if (filter == null)
            {
                return provider;
            }
            return new FilteredMetrics(provider, filter);
        }
    }
}