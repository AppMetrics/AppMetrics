// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;

namespace App.Metrics
{
    public interface IMeasureMeterMetrics
    {
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
    }
}