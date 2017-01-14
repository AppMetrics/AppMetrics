// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     Gets the record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    public interface IMetrics : ICounterMetrics
    {
        /// <summary>
        ///     Gets the more advanced application metrics operations
        /// </summary>
        /// <value>
        ///     The advanced.
        /// </value>
        /// <remarks>
        ///     -Gets the the regsitered <see cref="IClock" /> used for timing
        ///     - Allows retrieval of a snapshot of metrics data recorded via the <see cref="IMetricsDataProvider" /> and
        ///     reseting the data at runtime
        ///     - Allows retrieval of the current health of the application via the <see cref="IHealthStatusProvider" />
        ///     - Allows instantiation of a metric without performing the option
        ///     - Allows disabling of metrics recording at runtime
        /// </remarks>
        IAdvancedMetrics Advanced { get; }

        /// <summary>
        ///     Records <see cref="IGaugeMetric" /> which is a point in time instantaneous value
        /// </summary>
        /// <param name="options">The details of the gauge that is being measured.</param>
        /// <param name="valueProvider">A function that returns the value for the gauge.</param>
        void Gauge(GaugeOptions options, Func<double> valueProvider);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="amount">The amount to mark the meter.</param>
        void Mark(MeterOptions options, long amount);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events
        ///     overtime, will mark as 1.
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        void Mark(MeterOptions options);

        void Mark(MeterOptions options, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="item">The <see cref="MetricItem" />  within the set to mark.</param>
        void Mark(MeterOptions options, Action<MetricItem> item);

        void Mark(MeterOptions options, long amount, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="amount">The amount to mark the meter.</param>
        /// <param name="item">The <see cref="MetricItem" /> within the set to mark.</param>
        void Mark(MeterOptions options, long amount, Action<MetricItem> item);

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

        /// <summary>
        ///     Records a <see cref="IApdexMetric" /> which measures the time taken to process an action, samples data and procuded
        ///     an apdex score.
        /// </summary>
        /// <param name="options">The settings of the apdex metric that is being measured</param>
        /// <param name="action">The action to measure.</param>
        void Track(ApdexOptions options, Action action);

        /// <summary>
        ///     Records a <see cref="IApdexMetric" /> which measures the time taken to process an action, samples data and procuded
        ///     an apdex score.
        /// </summary>
        /// <param name="options">The settings of the apdex metric that is being measured</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        ApdexContext Track(ApdexOptions options);

        /// <summary>
        ///     Updates a <see cref="IHistogramMetric" /> which measures the distribution of values in a stream of data. Records
        ///     the min, mean,
        ///     max and standard deviation of values and also quantiles such as the medium, 95th percentile, 98th percentile, 99th
        ///     percentile and 99.9th percentile
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <param name="value">The value to be added to the histogram.</param>
        void Update(HistogramOptions options, long value);

        /// <summary>
        ///     Updates a <see cref="IHistogramMetric" /> which measures the distribution of values in a stream of data. Records
        ///     the min, mean,
        ///     max and standard deviation of values and also quantiles such as the medium, 95th percentile, 98th percentile, 99th
        ///     percentile and 99.9th percentile
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <param name="value">The value to be added to the histogram.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Update(HistogramOptions options, long value, string userValue);
    }
}