// <copyright file="MeterValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Meter
{
    /// <summary>
    ///     Combines the value of the meter with the defined unit and the rate unit at which the value is reported.
    /// </summary>
    public sealed class MeterValueSource : MetricValueSourceBase<MeterValue>
    {
        public MeterValueSource(
            string name,
            IMetricValueProvider<MeterValue> value,
            Unit unit,
            TimeUnit rateUnit,
            MetricTags tags,
            bool resetOnReporting = false,
            bool reportSetItems = true)
            : base(name, new ScaledValueProvider<MeterValue>(value, v => v.Scale(rateUnit)), unit, tags, resetOnReporting)
        {
            RateUnit = rateUnit;
            ReportSetItems = reportSetItems;
        }

        public TimeUnit RateUnit { get; }

        public bool ReportSetItems { get; }
    }
}
