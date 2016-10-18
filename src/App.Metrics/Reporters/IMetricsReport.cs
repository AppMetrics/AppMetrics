using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public interface IMetricsReport : IHideObjectMembers, IDisposable
    {
        IMetricsFilter Filter { get; }

        Task RunReport(IMetricsContext metricsContext, CancellationToken token);
    }
}