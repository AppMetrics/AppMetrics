// <copyright file="CustomTimer.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Timer;

namespace App.Metrics.Facts.TestHelpers
{
    public class CustomTimer : ITimer
    {
        /// <inheritdoc />
        public long CurrentTime()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public long EndRecording()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TimerContext NewContext(string userValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TimerContext NewContext()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Record(long time, TimeUnit unit, string userValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Record(long time, TimeUnit unit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public long StartRecording()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Time(Action action, string userValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Time(Action action)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T Time<T>(Func<T> action, string userValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T Time<T>(Func<T> action)
        {
            throw new NotImplementedException();
        }
    }
}