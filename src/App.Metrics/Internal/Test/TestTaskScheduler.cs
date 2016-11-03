// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using App.Metrics.Scheduling;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Test
{
    internal class TestTaskScheduler : IScheduler
    {
        private readonly IClock _clock;
        private Action _action;
        private long _lastRun = 0;
        private TimeSpan _pollInterval;

        public TestTaskScheduler(IClock clock)
        {
            if (clock == null) throw new ArgumentNullException(nameof(clock));

            _clock = clock;
            _clock.Advanced += (s, l) => RunIfNeeded();
        }

        public void Dispose()
        {
        }

        public Task Interval(TimeSpan pollInterval, Action action, CancellationToken token)
        {
            _pollInterval = pollInterval;
            _lastRun = _clock.Seconds;
            _action = action;
            return AppMetricsTaskCache.EmptyTask;
        }

        public Task Interval(TimeSpan pollInterval, Action action)
        {
            return Interval(pollInterval, action, CancellationToken.None);
        }

        public void Stop()
        {
        }

        private void RunIfNeeded()
        {
            var clockSeconds = _clock.Seconds;
            var elapsed = clockSeconds - _lastRun;
            var times = elapsed / _pollInterval.TotalSeconds;

            using (new CancellationTokenSource())
            {
                while (times-- >= 1)
                {
                    _lastRun = clockSeconds;
                    _action();
                }
            }
        }
    }
}