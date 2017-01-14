// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Data;

namespace App.Metrics
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
    /// <seealso cref="App.Metrics.Metric" />
    public sealed class Gauge : Metric
    {
        private double _value;

        public double? Value
        {
            get { return _value; }
            set { _value = value ?? double.NaN; }
        }

        public static Gauge FromGauge(MetricValueSource<double> gauge)
        {
            return new Gauge
                   {
                       Name = gauge.Name,
                       Value = gauge.Value,
                       Unit = gauge.Unit.Name,
                       Tags = gauge.Tags
                   };
        }
    }
}