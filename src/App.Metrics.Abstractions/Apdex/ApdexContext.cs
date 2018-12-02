// <copyright file="ApdexContext.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Apdex
{
    public struct ApdexContext : IDisposable
    {
        private readonly long _start;
        private IApdex _apdex;

        public ApdexContext(IApdex apdex)
        {
            _start = apdex.StartRecording();
            _apdex = apdex;
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
                if (_apdex == null)
                {
                    return TimeSpan.Zero;
                }

                var milliseconds = TimeUnit.Nanoseconds.Convert(TimeUnit.Milliseconds, _apdex.CurrentTime() - _start);
                return TimeSpan.FromMilliseconds(milliseconds);
            }
        }

        public void Dispose()
        {
            if (_apdex == null)
            {
                return;
            }

            var end = _apdex.EndRecording();

            var duration = end - _start;

            _apdex.Track(duration);

            _apdex = null;
        }
    }
}