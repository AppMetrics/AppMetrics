using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Utils;

namespace App.Metrics.Benchmarks
{
    public sealed class TestScheduler : IScheduler
    {
        private readonly IClock _clock;
        private Action<CancellationToken> _action;
        private TimeSpan _interval;
        private long _lastRun = 0;

        public TestScheduler(IClock clock)
        {
            _clock = clock;
            _clock.Advanced += (s, l) => RunIfNeeded();
        }

        public void Dispose()
        {
        }

        public void Start(TimeSpan interval, Func<CancellationToken, Task> task)
        {
            Start(interval, (t) => task(t).Wait(t));
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
                throw new ArgumentException("interval must be > 0 seconds", nameof(interval));
            }

            _interval = interval;
            _lastRun = _clock.Seconds;
            _action = action;
        }

        public void Stop()
        {
        }

        private void RunIfNeeded()
        {
            var clockSeconds = _clock.Seconds;
            var elapsed = clockSeconds - _lastRun;
            var times = elapsed / _interval.TotalSeconds;
            using (var ts = new CancellationTokenSource())
                while (times-- >= 1)
                {
                    _lastRun = clockSeconds;
                    _action(ts.Token);
                }
        }
    }
}