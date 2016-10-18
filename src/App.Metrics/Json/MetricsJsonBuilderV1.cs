using System.Globalization;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    internal class MetricsJsonBuilderV1 : IMetricsJsonBuilder
    {
        public const int Version = 1;

        private const bool DefaultIndented = true;

        public string MetricsMimeType { get; } = "application/vnd.app.metrics.v1.metrics+json";

        public string BuildJson(IMetricsContext metricsContext, EnvironmentInfo environmentInfo,
            IMetricsFilter filter)
        {
            return BuildJson(metricsContext, environmentInfo, filter, DefaultIndented);
        }

        public string BuildJson(IMetricsContext metricsContext, EnvironmentInfo environmentInfo, 
            IMetricsFilter filter, bool indented)
        {
          
            var version = Version.ToString(CultureInfo.InvariantCulture);
            var metricsData = metricsContext.Advanced.MetricsDataProvider
                .GetMetricsData(metricsContext)
                .Filter(filter);

            return JsonMetricsContext.FromContext(metricsData, environmentInfo, version)
                .ToJsonObject(metricsContext.Advanced.Clock)
                .AsJson(indented);
        }
    }
}