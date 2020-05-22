// <copyright file="IMeasureTimerMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Timer
{
    /// <summary>
    ///     Provides access to the API allowing Timer Metrics to be measured/recorded.
    /// </summary>
    public interface IMeasureTimerMetrics
    {
        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <param name="action">The action to measure.</param>
        void Time(TimerOptions options, Action action);

        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="action">The action to measure.</param>
        void Time(TimerOptions options, MetricTags tags, Action action);

        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <param name="action">The action to measure.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Time(TimerOptions options, Action action, string userValue);

        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="action">The action to measure.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Time(TimerOptions options, MetricTags tags, Action action, string userValue);

        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        TimerContext Time(TimerOptions options, MetricTags tags, string userValue);

        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        TimerContext Time(TimerOptions options, string userValue);

        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        TimerContext Time(TimerOptions options, MetricTags tags);

        /// <summary>
        ///     Records a <see cref="ITimerMetric" /> which measures the time taken to process an action using a timer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the timer that is being measured</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        TimerContext Time(TimerOptions options);

        /// <summary>
        /// Records a <see cref="ITimerMetric" /> which records the user generated time taken. This can be useful
        /// for an external service that you want to capture its observed duration.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="tags"></param>
        /// <param name="time"></param>
        void Time(TimerOptions options, MetricTags tags, long time);

        /// <summary>
        /// Records a <see cref="ITimerMetric" /> which records the user generated time taken. This can be useful
        /// for an external service that you want to capture its observed duration.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="time"></param>
        void Time(TimerOptions options, long time);
    }
}