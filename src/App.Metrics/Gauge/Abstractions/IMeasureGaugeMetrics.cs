// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Options;

namespace App.Metrics.Gauge.Abstractions
{
    /// <summary>
    ///     Provides access to the API allowing Gauge Metrics to be measured/recorded.
    /// </summary>
    public interface IMeasureGaugeMetrics
    {
        /// <summary>
        ///     Records <see cref="IGaugeMetric" /> which is a point in time instantaneous value
        /// </summary>
        /// <param name="options">The details of the gauge that is being measured.</param>
        /// <param name="valueProvider">A function that returns custom value provider for the gauge.</param>
        void SetValue(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider);

        /// <summary>
        ///     Records <see cref="IGaugeMetric" /> which is a point in time instantaneous value
        /// </summary>
        /// <param name="options">The details of the gauge that is being measured.</param>
        /// <param name="valueProvider">A function that returns the value for the gauge.</param>
        void SetValue(GaugeOptions options, Func<double> valueProvider);
    }
}