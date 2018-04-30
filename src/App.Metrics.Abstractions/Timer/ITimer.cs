// <copyright file="ITimer.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace App.Metrics.Timer
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
        TimerContext NewContext(string userValue);

        /// <summary>
        ///     Creates a new disposable instance and records the time it takes until the instance is disposed.
        ///     <code>
        /// using(timer.NewContext())
        /// {
        ///     ExecuteMethodThatNeedsMonitoring();
        /// }
        /// </code>
        /// </summary>
        /// <returns>A disposable instance that will record the time passed until disposed.</returns>
        TimerContext NewContext();

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
        void Record(long time, TimeUnit unit, string userValue);

        /// <summary>
        ///     This is part of advanced timer API. Use Timer.NewContext() by default.
        ///     Manually record timer value.
        /// </summary>
        /// <param name="time">The value representing the manually measured time.</param>
        /// <param name="unit">Unit for the value.</param>
        void Record(long time, TimeUnit unit);

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
        void Time(Action action, string userValue);

        /// <summary>
        ///     Runs the <paramref name="action" /> and records the time it took.
        /// </summary>
        /// <param name="action">Action to run and record time for.</param>
        void Time(Action action);

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
        T Time<T>(Func<T> action, string userValue);

        /// <summary>
        ///     Runs the <paramref name="action" /> returning the result and records the time it took.
        /// </summary>
        /// <typeparam name="T">Type of the value returned by the action</typeparam>
        /// <param name="action">Action to run and record time for.</param>
        /// <returns>The result of the <paramref name="action" /></returns>
        T Time<T>(Func<T> action);
    }
}