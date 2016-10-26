using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting
{
    public interface IReporter
    {
        Task RunReports(IMetricsContext context, CancellationToken token);
    }
}