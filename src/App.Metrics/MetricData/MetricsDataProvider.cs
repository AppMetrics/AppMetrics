using App.Metrics.Utils;

namespace App.Metrics.MetricData
{
    /// <summary>
    ///     A provider capable of returning the current values for a set of metrics
    /// </summary>
    public interface MetricsDataProvider : IHideObjectMembers
    {
        /// <summary>
        ///     Returns the current metrics data for the context for which this provider has been created.
        /// </summary>
        MetricsData CurrentMetricsData { get; }
    }

    public sealed class FilteredMetrics : MetricsDataProvider
    {
        private readonly MetricsFilter filter;
        private readonly MetricsDataProvider provider;

        public FilteredMetrics(MetricsDataProvider provider, MetricsFilter filter)
        {
            this.provider = provider;
            this.filter = filter;
        }

        public MetricsData CurrentMetricsData
        {
            get { return this.provider.CurrentMetricsData.Filter(this.filter); }
        }
    }

    public static class FilteredMetricsExtensions
    {
        public static MetricsDataProvider WithFilter(this MetricsDataProvider provider, MetricsFilter filter)
        {
            if (filter == null)
            {
                return provider;
            }
            return new FilteredMetrics(provider, filter);
        }
    }
}