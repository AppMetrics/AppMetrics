// <copyright file="IMetricValueProvider{T}.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    /// <summary>
    ///     Indicates the ability to provide the value for a metric.
    ///     This is the raw value. Consumers should use MetricValueSourceBase{T} />
    /// </summary>
    /// <typeparam name="T">Type of the value returned by the metric</typeparam>
    public interface IMetricValueProvider<out T>
    {
        /// <summary>
        ///     Gets the current value of the metric.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        T Value { get; }

        /// <summary>
        ///     Get the current value for the metric, but also reset the metric.
        ///     Useful for pushing data to only one consumer (ex: graphite) where you might want to only capture values just
        ///     between the report interval.
        /// </summary>
        /// <param name="resetMetric">if set to true the metric will be reset.</param>
        /// <returns>The current value for the metric.</returns>
        T GetValue(bool resetMetric = false);
    }
}