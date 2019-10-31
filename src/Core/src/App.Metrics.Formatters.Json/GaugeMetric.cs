// <copyright file="GaugeMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     <para>
    ///         Gauge metric types measure the value of a particular thing at a particular time, it represents an instantaneous
    ///         value.
    ///     </para>
    ///     <para>
    ///         Gauges represent a double value.
    ///     </para>
    /// </summary>
    /// <seealso cref="MetricBase" />
    public sealed class GaugeMetric : MetricBase
    {
        private double _value;

        public double? Value
        {
            get => _value;
            // TODO: https://github.com/AppMetrics/AppMetrics/issues/502
            // set => _value = value ?? double.NaN;
            set => _value = value ?? 0;
        }

        public static GaugeMetric FromGauge(MetricValueSourceBase<double> gauge)
        {
            return new GaugeMetric
                   {
                       Name = gauge.Name,
                       Value = gauge.Value,
                       Unit = gauge.Unit.Name,
                       Tags = gauge.Tags.ToDictionary()
            };
        }
    }
}