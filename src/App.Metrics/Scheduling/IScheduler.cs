using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Scheduling
{
    public interface IScheduler : IDisposable
    {
        Task Interval(TimeSpan pollInterval, Action action, CancellationToken token);

        Task Interval(TimeSpan pollInterval, Action action);

        void Stop();
    }
}