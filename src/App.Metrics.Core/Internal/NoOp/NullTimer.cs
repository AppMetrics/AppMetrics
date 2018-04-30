// <copyright file="NullTimer.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Timer;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public struct NullTimer : ITimer
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

        public void Time(Action action) { action(); }

        public T Time<T>(Func<T> action) { return action(); }

        TimerContext ITimer.NewContext() { return new TimerContext(this, null); }
    }
}