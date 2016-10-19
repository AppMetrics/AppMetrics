using System.Threading;

namespace App.Metrics.Reporters
{
    public interface IScheduledReporter
    {
        void Dispose();

        void Start(CancellationToken token);
    }
}