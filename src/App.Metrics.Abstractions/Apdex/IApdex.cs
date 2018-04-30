// <copyright file="IApdex.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Apdex
{
    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Apdex">Apdex</see> allows us to measure an apdex score which is a ratio of
    ///     the number of satisfied and tolerating requests to the total requests made. Each satisfied request counts as one
    ///     request, while each tolerating request counts as half a satisfied request.
    ///     <para>
    ///         Apdex tracks three response counts, counts based on samples measured by the chosen
    ///         <see cref="IReservoir">reservoir</see>.
    ///     </para>
    ///     <para>
    ///         Satisfied, Tolerated and Frustrated request counts are calculated as follows using a user value of T seconds.
    ///     </para>
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 Satisfied: T or less
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Tolerated: Greater than T or less than 4T
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 Frustrated: Greater than 4 T
    ///             </description>
    ///         </item>
    ///     </list>
    /// </summary>
    /// <seealso>
    ///     <cref>App.Metrics.IResetableMetric</cref>
    /// </seealso>
    public interface IApdex : IResetableMetric
    {
        /// <summary>
        ///     This is part of advanced timer API. Use Timer.NewContext() by default.
        ///     Returns the current time in nanoseconds for computing elapsed time.
        /// </summary>
        /// <returns>value representing the current time in nanoseconds.</returns>
        long CurrentTime();

        /// <summary>
        ///     This is part of advanced timer API. Use Apdex.NewContext() by default.
        ///     Manually ends timing an action.
        /// </summary>
        /// <returns>value representing the current time in nanoseconds.</returns>
        long EndRecording();

        /// <summary>
        ///     Creates a new disposable instance and records the time it takes until the instance is disposed.
        ///     <code>
        /// using(apdex.NewContext())
        /// {
        ///     ExecuteMethodThatNeedsMonitoring();
        /// }
        /// </code>
        /// </summary>
        /// <returns>A disposable instance that will record the time passed until disposed.</returns>
        ApdexContext NewContext();

        /// <summary>
        ///     This is part of advanced apdex API. Use Apdex.NewContext() by default.
        ///     Manually start timing an action.
        /// </summary>
        /// <returns>value representing the current time in nanoseconds.</returns>
        long StartRecording();

        /// <summary>
        ///     This is part of advanced apdex API. Use Apdex.NewContext() by default.
        ///     Manually record timer value use to calculate the apdex score.
        /// </summary>
        /// <param name="duration">The value representing the manually measured time.</param>
        void Track(long duration);

        /// <summary>
        ///     Runs the <paramref name="action" /> and records the time it took allowing us to calculate an apdex score.
        /// </summary>
        /// <param name="action">Action to run and record time for.</param>
        void Track(Action action);

        /// <summary>
        ///     Runs the <paramref name="action" /> returning the result and records the time it took allowing us to calculate an
        ///     apdex score.
        /// </summary>
        /// <typeparam name="T">Type of the value returned by the action</typeparam>
        /// <param name="action">Action to run and record time for.</param>
        /// <returns>The result of the <paramref name="action" /></returns>
        T Track<T>(Func<T> action);
    }
}