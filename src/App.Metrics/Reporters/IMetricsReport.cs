using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public interface IMetricsReport : IHideObjectMembers, IDisposable
    {
        Task RunReport(MetricsData metricsData, IHealthCheckDataProvider healthCheckDataProvider, CancellationToken token);
    }
}