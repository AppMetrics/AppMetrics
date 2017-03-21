// <copyright file="NullTimer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Internal;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal struct NullTimer : ITimer
    {
        public long CurrentTime() { return 0; }

        public long EndRecording() { return 0; }

        public TimerContext NewContext(string userValue) { return new TimerContext(this, null); }

        public void Record(long time, TimeUnit unit, string userValue) { }

        public void Record(long time, TimeUnit unit) { }

        public void Reset() { }

        public long StartRecording() { return 0; }

        public void Time(Action action, string userValue) { action(); }

        public T Time<T>(Func<T> action, string userValue) { return action(); }

        public void Time(Action action) { }

        public T Time<T>(Func<T> action) { return action(); }

        TimerContext ITimer.NewContext() { return new TimerContext(this, null); }
    }
}