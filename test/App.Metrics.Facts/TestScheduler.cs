using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Utils;

namespace App.Metrics.Facts
{
    /// <summary>
    ///     Utility class for manually executing the scheduled task.
    /// </summary>
    /// <remarks>
    ///     This class is useful for testing.
    /// </remarks>
    public sealed class TestScheduler : IScheduler
    {
        private readonly TestClock clock;
        private Action<CancellationToken> action;
        private TimeSpan interval;
        private long lastRun = 0;

        public TestScheduler(TestClock clock)
        {
            this.clock = clock;
            this.clock.Advanced += (s, l) => this.RunIfNeeded();
        }

        public void Dispose()
        {
        }

        public void Start(TimeSpan interval, Func<CancellationToken, Task> task)
        {
            Start(interval, (t) => task(t).Wait());
        }

        public void Start(TimeSpan interval, Func<Task> task)
        {
            Start(interval, () => task().Wait());
        }

        public void Start(TimeSpan interval, Action action)
        {
            Start(interval, t => action());
        }

        public void Start(TimeSpan interval, Action<CancellationToken> action)
        {
            if (interval.TotalSeconds == 0)
            {
                throw new ArgumentException("interval must be > 0 seconds", "interval");
            }

            this.interval = interval;
            this.lastRun = this.clock.Seconds;
            this.action = action;
        }

        public void Stop()
        {
        }

        private void RunIfNeeded()
        {
            var clockSeconds = clock.Seconds;
            var elapsed = clockSeconds - lastRun;
            var times = elapsed / interval.TotalSeconds;
            using (var ts = new CancellationTokenSource())
                while (times-- >= 1)
                {
                    lastRun = clockSeconds;
                    action(ts.Token);
                }
        }
    }
}