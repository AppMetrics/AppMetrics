using System;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public interface MetricsReport : IHideObjectMembers
    {
        void RunReport(MetricsData metricsData, Func<HealthStatus> healthStatus, CancellationToken token);
    }
}