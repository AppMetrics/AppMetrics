// <copyright file="TimerContext.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Timer
{
    /// <summary>
    ///     This struct is meant to be returned by the timer.NewContext() method and is intended to be used inside a using
    ///     statement:
    ///     <code>
    /// using(timer.NewContext())
    /// {
    ///     ExecuteMethodThatNeedsMonitoring();
    /// }
    /// </code>
    ///     <remarks>
    ///         Double disposing the instance, or a copy of the instance (since it is a struct) will cause the timer to record
    ///         wrong values.
    ///         Stick to directly passing it to the using() statement.
    ///     </remarks>
    /// </summary>
    public struct TimerContext : IDisposable
    {
        private readonly long _start;
        private ITimer _timer;
        private string _userValue;

        public TimerContext(ITimer timer, string userValue)
        {
            _start = timer.StartRecording();
            _timer = timer;
            _userValue = userValue;
        }

        /// <summary>
        ///     Gets the currently elapsed time from when the instance has been created
        /// </summary>
        /// <value>
        ///     The elapsed.
        /// </value>
        public TimeSpan Elapsed
        {
            get
            {
                if (_timer == null)
                {
                    return TimeSpan.Zero;
                }

                var milliseconds = TimeUnit.Nanoseconds.Convert(TimeUnit.Milliseconds, _timer.CurrentTime() - _start);
                return TimeSpan.FromMilliseconds(milliseconds);
            }
        }

        public void Dispose()
        {
            if (_timer == null)
            {
                return;
            }

            var end = _timer.EndRecording();
            _timer.Record(end - _start, TimeUnit.Nanoseconds, _userValue);
            _timer = null;
        }

        /// <summary>
        ///     Set the user value for this timer context.
        /// </summary>
        /// <param name="value">New user value to use for this context.</param>
        public void TrackUserValue(string value) { _userValue = value; }
    }
}