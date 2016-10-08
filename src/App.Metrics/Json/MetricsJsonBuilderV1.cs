using System;
using System.Globalization;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    internal class MetricsJsonBuilderV1 : IMetricsJsonBuilder
    {
        public const int Version = 1;

        private const bool DefaultIndented = true;
        private readonly IClock _systemClock;

        public MetricsJsonBuilderV1(IClock systemClock)
        {
            if (systemClock == null)
            {
                throw new ArgumentNullException(nameof(systemClock));
            }

            _systemClock = systemClock;
        }

        public string MetricsMimeType { get; } = "application/vnd.app.metrics.v1.metrics+json";

        public string BuildJson(MetricsData data, EnvironmentInfo environmentInfo)
        {
            return BuildJson(data, environmentInfo, _systemClock, indented: DefaultIndented);
        }

        public string BuildJson(MetricsData data, EnvironmentInfo environmentInfo, IClock clock, bool indented = DefaultIndented)
        {
            var version = Version.ToString(CultureInfo.InvariantCulture);

            return JsonMetricsContext.FromContext(data, environmentInfo, version)
                .ToJsonObject()
                .AsJson(indented);
        }
    }
}