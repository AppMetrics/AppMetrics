// <copyright file="IMeasureGaugeMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Tagging;

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
        void SetValue(GaugeOptions options, Func<double> valueProvider);

        void SetValue(GaugeOptions options, double value);

        void SetValue(GaugeOptions options, MetricTags tags, double value);

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
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique
        ///     <see cref="MetricTags" />
        /// </param>
        /// <param name="valueProvider">A function that returns custom value provider for the gauge.</param>
        void SetValue(GaugeOptions options, MetricTags tags, Func<IMetricValueProvider<double>> valueProvider);

        /// <summary>
        ///     Records <see cref="IGaugeMetric" /> which is a point in time instantaneous value
        /// </summary>
        /// <param name="options">The details of the gauge that is being measured.</param>
        /// <param name="tags">
        ///     The runtime tags to set in addition to those defined on the options, this will create a separate metric per unique
        ///     <see cref="MetricTags" />
        /// </param>
        /// <param name="valueProvider">A function that returns the value for the gauge.</param>
        void SetValue(GaugeOptions options, MetricTags tags, Func<double> valueProvider);
    }
}