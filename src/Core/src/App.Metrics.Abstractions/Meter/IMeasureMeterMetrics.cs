// <copyright file="IMeasureMeterMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Meter
{
    /// <summary>
    ///     Provides access to the API allowing Meter Metrics to be measured/recorded.
    /// </summary>
    public interface IMeasureMeterMetrics
    {
        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        void Mark(MeterOptions options);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="amount">The amount to mark the meter.</param>
        void Mark(MeterOptions options, long amount);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="amount">The amount to mark the meter.</param>
        void Mark(MeterOptions options, MetricTags tags, long amount);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events
        ///     overtime, will mark as 1.
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        void Mark(MeterOptions options, MetricTags tags);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="item">The metric item within the set to mark.</param>
        void Mark(MeterOptions options, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="item">The metric item within the set to mark.</param>
        void Mark(MeterOptions options, MetricTags tags, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="setItem">The <see cref="MetricSetItem" />  within the set to mark.</param>
        void Mark(MeterOptions options, MetricSetItem setItem);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="setItem">The <see cref="MetricSetItem" />  within the set to mark.</param>
        void Mark(MeterOptions options, MetricTags tags, MetricSetItem setItem);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="amount">The amount to mark the meter.</param>
        /// <param name="item">The metric item within the set to mark.</param>
        void Mark(MeterOptions options, long amount, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="amount">The amount to mark the meter.</param>
        /// <param name="item">The metric item within the set to mark.</param>
        void Mark(MeterOptions options, MetricTags tags, long amount, string item);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="amount">The amount to mark the meter.</param>
        /// <param name="setItem">The <see cref="MetricSetItem" /> within the set to mark.</param>
        void Mark(MeterOptions options, long amount, MetricSetItem setItem);

        /// <summary>
        ///     Marks a <see cref="IMeterMetric" /> which increments an increment-only counter and measures the rate of events over
        ///     time
        /// </summary>
        /// <param name="options">The details of the meter that is being marked</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique <see cref="MetricTags"/>
        /// </param>
        /// <param name="amount">The amount to mark the meter.</param>
        /// <param name="setItem">The <see cref="MetricSetItem" /> within the set to mark.</param>
        void Mark(MeterOptions options, MetricTags tags, long amount, MetricSetItem setItem);
    }
}