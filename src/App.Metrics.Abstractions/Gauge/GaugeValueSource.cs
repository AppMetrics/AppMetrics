// <copyright file="GaugeValueSource.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Gauge
{
    /// <summary>
    ///     Combines the value of a gauge (a double) with the defined unit for the value.
    /// </summary>
    public sealed class GaugeValueSource : MetricValueSourceBase<double>
    {
        public GaugeValueSource(
            string name,
            IMetricValueProvider<double> value,
            Unit unit,
            MetricTags tags)
            : base(name, value, unit, tags)
        {
        }
    }
}