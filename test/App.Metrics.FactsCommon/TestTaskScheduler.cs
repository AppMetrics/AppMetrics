// <copyright file="TestTaskScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Scheduling;

namespace App.Metrics.FactsCommon
{
    public sealed class TestTaskScheduler : IScheduler
    {
        private readonly IClock _clock;
        private Action _action;
        private long _lastRun;
        private TimeSpan _pollInterval;

        public TestTaskScheduler(IClock clock)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _clock.Advanced += (s, l) => RunIfNeeded();
        }

        public void Dispose() { }

        // <inheritdoc />
        public Task Interval(
            TimeSpan pollInterval,
            TaskCreationOptions taskCreationOptions,
            Action action,
            CancellationToken token)
        {
            _pollInterval = pollInterval;
            _lastRun = _clock.Seconds;
            _action = action;
            return Task.CompletedTask;
        }

        // <inheritdoc />
        public Task Interval(
            TimeSpan pollInterval,
            TaskCreationOptions taskCreationOptions,
            Action action) { return Interval(pollInterval, taskCreationOptions, action, CancellationToken.None); }

        // <inheritdoc />
        public void Stop() { }

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