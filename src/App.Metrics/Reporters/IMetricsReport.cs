using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public interface IMetricsReport : IHideObjectMembers
    {
        void RunReport(MetricsData metricsData, Func<Task<HealthStatus>> healthStatus, CancellationToken token);
    }
}