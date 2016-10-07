using System;
using System.Collections.Generic;
using System.Globalization;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public class MetricsJsonBuilderV1 : IMetricsJsonBuilder
    {
        public const string MetricsMimeType = "application/vnd.app.metrics.v1.metrics+json";
        public const int Version = 1;

        private const bool DefaultIndented = true;
        private readonly AppEnvironment _appEnvironment;

        public MetricsJsonBuilderV1(AppEnvironment appEnvironment)
        {
            if (appEnvironment == null)
            {
                throw new ArgumentNullException(nameof(appEnvironment));
            }

            _appEnvironment = appEnvironment;
        }

        public string BuildJson(MetricsData data)
        {
            return BuildJson(data, _appEnvironment.Current, Clock.Default, indented: DefaultIndented);
        }

        public string BuildJson(MetricsData data, IEnumerable<EnvironmentEntry> environment, Clock clock, bool indented = DefaultIndented)
        {
            var version = Version.ToString(CultureInfo.InvariantCulture);

            return JsonMetricsContext.FromContext(data, environment, version)
                .ToJsonObject()
                .AsJson(indented);
        }
    }
}