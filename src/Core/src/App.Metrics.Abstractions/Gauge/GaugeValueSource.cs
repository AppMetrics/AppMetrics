// <copyright file="GaugeValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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
            MetricTags tags,
            bool restOnReporting = false)
            : base(name, value, unit, tags, restOnReporting)
        {
        }
    }
}
