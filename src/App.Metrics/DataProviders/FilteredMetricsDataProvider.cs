using App.Metrics.MetricData;

namespace App.Metrics.DataProviders
{
    public sealed class FilteredMetricsDataProvider : IMetricsDataProvider
    {
        private readonly IMetricsFilter _filter;
        private readonly IMetricsDataProvider _provider;

        public FilteredMetricsDataProvider(IMetricsDataProvider provider, IMetricsFilter filter)
        {
            _provider = provider;
            _filter = filter;
        }

        public MetricsData CurrentMetricsData => _provider.CurrentMetricsData.Filter(_filter);
    }
}