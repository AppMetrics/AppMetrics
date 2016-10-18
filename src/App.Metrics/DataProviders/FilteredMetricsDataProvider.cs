using System;
using App.Metrics.MetricData;

namespace App.Metrics.DataProviders
{
    public sealed class FilteredMetricsDataProvider : IMetricsDataProvider
    {
        private readonly IMetricsFilter _filter;
        private readonly IMetricsDataProvider _provider;

        public FilteredMetricsDataProvider(IMetricsDataProvider provider, IMetricsFilter filter)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _provider = provider;
            _filter = filter;
        }

        public MetricsData GetMetricsData(IMetricsContext metricsContext)
        {
            return _provider.GetMetricsData(metricsContext).Filter(_filter);
        }
    }
}