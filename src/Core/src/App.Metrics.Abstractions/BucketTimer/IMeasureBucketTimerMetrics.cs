// <copyright file="IMeasureBucketTimerMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Timer;

namespace App.Metrics.BucketTimer
{
    /// <summary>
    ///     Provides access to the API allowing BucketTimer Metrics to be measured/recorded.
    /// </summary>
    public interface IMeasureBucketTimerMetrics
    {
        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <param name="action">The action to measure.</param>
        void Time(BucketTimerOptions options, Action action);

        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="action">The action to measure.</param>
        void Time(BucketTimerOptions options, MetricTags tags, Action action);

        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <param name="action">The action to measure.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Time(BucketTimerOptions options, Action action, string userValue);

        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="action">The action to measure.</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        void Time(BucketTimerOptions options, MetricTags tags, Action action, string userValue);

        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        TimerContext Time(BucketTimerOptions options, MetricTags tags, string userValue);

        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <param name="userValue">The user value to track where a Min, Max and Last duration is recorded.</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        TimerContext Time(BucketTimerOptions options, string userValue);

        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        TimerContext Time(BucketTimerOptions options, MetricTags tags);

        /// <summary>
        ///     Records a <see cref="IBucketTimerMetric" /> which measures the time taken to process an action using a BucketTimer metric.
        ///     Records a histogram of the duration of a type of event and a meter of the rate of it's occurance
        /// </summary>
        /// <param name="options">The details of the BucketTimer that is being measured</param>
        /// <returns>A disposable context, when disposed records the time token to process the using block</returns>
        TimerContext Time(BucketTimerOptions options);
    }
}