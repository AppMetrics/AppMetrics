// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;

namespace App.Metrics.Timer.Abstractions
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
        /// <param name="action">The action to measure.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Time(TimerOptions options, Action action, string userValue);

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
        TimerContext Time(TimerOptions options);
    }
}