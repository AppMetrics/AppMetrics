using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;

namespace App.Metrics.Reporting
{
    internal sealed class NullReporter : IReporter
    {
        public Task RunReports(IMetrics context, CancellationToken token)
        {
            return AppMetricsTaskCache.EmptyTask;
        }
    }
}