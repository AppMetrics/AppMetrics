// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     Provides access to record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    public interface IMetrics : IHideObjectMembers
    {
        /// <summary>
        ///     Provides access to more advanced application metrics operations
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         - Provides access to the regsitered <see cref="IClock" /> used for timing
        ///         - Allows retrieval of a snapshot of metrics data recorded via the <see cref="IMetricsDataProvider" /> and
        ///         reseting the data at runtime
        ///         - Allows retrieval of the current health of the application via the <see cref="IHealthStatusProvider" />
        ///         - Allows instantiation of a metric without performing the option
        ///         - Allows disabling of metrics recording at runtime
        ///     </para>
        /// </remarks>
        IAdvancedMetrics Advanced { get; }

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        void Decrement(CounterOptions options);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" /> by the specificed amount
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="amount">The amount to decrement the counter.</param>
        void Decrement(CounterOptions options, long amount);

        //TODO: AH - keeping this method?
        void Decrement(CounterOptions options, string item);

        //TODO: AH - keeping this method?
        void Decrement(CounterOptions options, long amount, string item);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <remarks>
        ///     The counter value is decremented as is the specified <see cref="MetricItem" />'s counter within the set.
        ///     The <see cref="MetricItem" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="item">The item within the set to decrement.</param>
        void Decrement(CounterOptions options, Action<MetricItem> item);

        /// <summary>
        ///     Decrements a <see cref="ICounterMetric" /> by the specified amount as well as the specified item within the
        ///     counter's set
        /// </summary>
        /// <param name="options">The details of the counter that is being decremented</param>
        /// <param name="amount">The amount to decrement the counter.</param>
        /// <param name="item">The item within the set to decrement.</param>
        /// <remarks>
        ///     The counter value is decremented as is the specified <see cref="MetricItem" />'s counter within the set.
        ///     The <see cref="MetricItem" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        void Decrement(CounterOptions options, long amount, Action<MetricItem> item);

        /// <summary>
        ///     Records <see cref="IGaugeMetric" /> which is a point in time instantaneous value
        /// </summary>
        /// <param name="options">The details of the gauge that is being measured.</param>
        /// <param name="valueProvider">A function that returns the value for the gauge.</param>
        void Gauge(GaugeOptions options, Func<double> valueProvider);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        void Increment(CounterOptions options);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" />
        /// </summary>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="amount">The amount to decrement the counter.</param>
        void Increment(CounterOptions options, long amount);

        /// <summary>
        ///     Increments a <see cref="ICounterMetric" /> as well as the specified item within the counter's set
        /// </summary>
        /// <remarks>
        ///     The counter value is incremented as is the specified <see cref="MetricItem" />'s counter within the set.
        ///     The <see cref="MetricItem" /> within the set will also keep track of it's percentage from the total sets count.
        /// </remarks>
        /// <param name="options">The details of the counter that is being incremented</param>
        /// <param name="item">The item within the set to increment.</param>
        void Increment(CounterOptions options, string item);

        //TODO: AH - keeping this method?
        void Increment(CounterOptions options, long amount, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="amount">The amount to mark the meter.</param>
        void Mark(MeterOptions options, long amount);

        //TODO: AH - keeping this method?
        void Mark(MeterOptions options, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="item">The <see cref="MetricItem" />  within the set to mark.</param>
        void Mark(MeterOptions options, Action<MetricItem> item);

        //TODO: AH - keeping this method?
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
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        void Mark(MeterOptions options);

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