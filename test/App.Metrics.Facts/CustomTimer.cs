// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Facts
{
    public class CustomTimer : ITimer
    {
        /// <inheritdoc />
        public long CurrentTime() { throw new NotImplementedException(); }

        /// <inheritdoc />
        public long EndRecording() { throw new NotImplementedException(); }

        /// <inheritdoc />
        public TimerContext NewContext(string userValue) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public TimerContext NewContext() { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Record(long time, TimeUnit unit, string userValue) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Record(long time, TimeUnit unit) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Reset() { throw new NotImplementedException(); }

        /// <inheritdoc />
        public long StartRecording() { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Time(Action action, string userValue) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Time(Action action) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public T Time<T>(Func<T> action, string userValue) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public T Time<T>(Func<T> action) { throw new NotImplementedException(); }
    }
}