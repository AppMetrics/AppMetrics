using System.Collections.Generic;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public interface IMetricsJsonBuilder
    {
        string BuildJson(MetricsData data);

        string BuildJson(MetricsData data, IEnumerable<EnvironmentEntry> environment, Clock clock, bool indented = true);
    }
}