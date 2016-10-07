using System;

namespace App.Metrics
{
    /// <summary>
    ///     A timer is basically a histogram of the duration of a type of event and a meter of the rate of its occurrence.
    ///     <seealso cref="IHistogram" /> and <seealso cref="IMeter" />
    /// </summary>
    public interface ITimer : IResetableMetric
    {
        /// <summary>
        ///     This is part of advanced timer API. Use Timer.NewContext() by default.
        ///     Returns the current time in nanoseconds for computing elapsed time.
        /// </summary>
        /// <returns>value representing the current time in nanoseconds.</returns>
        long CurrentTime();

        /// <summary>
        ///     This is part of advanced timer API. Use Timer.NewContext() by default.
        ///     Manually ends timing an action.
        /// </summary>
        /// <returns>value representing the current time in nanoseconds.</returns>
        long EndRecording();

        /// <summary>
        ///     Creates a new disposable instance and records the time it takes until the instance is disposed.
        ///     <code>
        /// using(timer.NewContext())
        /// {
        ///     ExecuteMethodThatNeedsMonitoring();
        /// }
        /// </code>
        /// </summary>
        /// <param name="userValue">
        ///     A custom user value that will be associated to the results.
        ///     Useful for tracking (for example) for which id the max or min value was recorded.
        /// </param>
        /// <returns>A disposable instance that will record the time passed until disposed.</returns>
        TimerContext NewContext(string userValue = null);

        /// <summary>
        ///     This is part of advanced timer API. Use Timer.NewContext() by default.
        ///     Manually record timer value.
        /// </summary>
        /// <param name="time">The value representing the manually measured time.</param>
        /// <param name="unit">Unit for the value.</param>
        /// <param name="userValue">
        ///     A custom user value that will be associated to the results.
        ///     Useful for tracking (for example) for which id the max or min value was recorded.
        /// </param>
        void Record(long time, TimeUnit unit, string userValue = null);

        /// <summary>
        ///     This is part of advanced timer API. Use Timer.NewContext() by default.
        ///     Manually start timing an action.
        /// </summary>
        /// <returns>value representing the current time in nanoseconds.</returns>
        long StartRecording();

        /// <summary>
        ///     Runs the <paramref name="action" /> and records the time it took.
        /// </summary>
        /// <param name="action">Action to run and record time for.</param>
        /// <param name="userValue">
        ///     A custom user value that will be associated to the results.
        ///     Useful for tracking (for example) for which id the max or min value was recorded.
        /// </param>
        void Time(Action action, string userValue = null);

        /// <summary>
        ///     Runs the <paramref name="action" /> returning the result and records the time it took.
        /// </summary>
        /// <typeparam name="T">Type of the value returned by the action</typeparam>
        /// <param name="action">Action to run and record time for.</param>
        /// <param name="userValue">
        ///     A custom user value that will be associated to the results.
        ///     Useful for tracking (for example) for which id the max or min value was recorded.
        /// </param>
        /// <returns>The result of the <paramref name="action" /></returns>
        T Time<T>(Func<T> action, string userValue = null);
    }

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
        ///     Provides the currently elapsed time from when the instance has been created
        /// </summary>
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
            if (_timer != null)
            {
                var end = _timer.EndRecording();
                _timer.Record(end - _start, TimeUnit.Nanoseconds, _userValue);
                _timer = null;
            }
        }

        /// <summary>
        ///     Set the user value for this timer context.
        /// </summary>
        /// <param name="value">New user value to use for this context.</param>
        public void TrackUserValue(string value)
        {
            _userValue = value;
        }
    }
}