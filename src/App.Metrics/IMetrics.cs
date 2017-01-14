// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     Gets the record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    public interface IMetrics : IMeasureCounterMetrics,
        IMeasureGaugeMetrics,
        IMeasureMeterMetrics,
        IMeasureTimerMetrics,
        IMeasureApdexMetrics,
        IMeasureHistogramMetrics
    {
        /// <summary>
        ///     Gets the more advanced application metrics operations
        /// </summary>
        /// <value>
        ///     The advanced.
        /// </value>
        /// <remarks>
        ///     - Gets the the regsitered <see cref="IClock" /> used for timing
        ///     - Allows retrieval of a snapshot of metrics data recorded via the <see cref="IMetricsDataProvider" /> and
        ///     reseting the data at runtime
        ///     - Allows retrieval of the current health of the application via the <see cref="IHealthStatusProvider" />
        ///     - Allows instantiation of a metric without performing the option
        ///     - Allows disabling of metrics recording at runtime
        /// </remarks>
        IAdvancedMetrics Advanced { get; }
    }
}