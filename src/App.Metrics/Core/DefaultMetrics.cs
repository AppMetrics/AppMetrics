// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Configuration;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Core
{
    /// <summary>
    ///     Provides access to record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    /// <seealso cref="IMetrics" />
    internal sealed class DefaultMetrics : IMetrics
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetrics" /> class.
        /// </summary>
        /// <param name="options">The global metrics options configure on startup.</param>
        /// <param name="metricsManagerFactory">The metric types aggregate service.</param>
        /// <param name="advanced">The implementation providing access to more advanced metric options</param>
        /// <exception cref="ArgumentNullException">When any options, registry and/or advanced is null</exception>
        public DefaultMetrics(
            AppMetricsOptions options,
            IMetricsManagerFactory metricsManagerFactory,
            IAdvancedMetrics advanced)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (advanced == null)
            {
                throw new ArgumentNullException(nameof(advanced));
            }

            Apdex = metricsManagerFactory.ApdexManager;
            Gauge = metricsManagerFactory.GaugeManager;
            Counter = metricsManagerFactory.CounterManager;
            Meter = metricsManagerFactory.MeterManager;
            Histogram = metricsManagerFactory.HistogramManager;
            Timer = metricsManagerFactory.TimerManager;
            Advanced = advanced;
        }

        /// <inheritdoc />
        public IAdvancedMetrics Advanced { get; }

        /// <inheritdoc />
        public IMeasureApdexMetrics Apdex { get; }

        /// <inheritdoc />
        public IMeasureCounterMetrics Counter { get; }

        /// <inheritdoc />
        public IMeasureGaugeMetrics Gauge { get; }

        /// <inheritdoc />
        public IMeasureHistogramMetrics Histogram { get; }

        /// <inheritdoc />
        public IMeasureMeterMetrics Meter { get; }

        /// <inheritdoc />
        public IMeasureTimerMetrics Timer { get; }
    }
}