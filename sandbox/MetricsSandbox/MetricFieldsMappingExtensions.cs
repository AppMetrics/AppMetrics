// <copyright file="MetricFieldsMappingExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics;

namespace MetricsSandbox
{
    public static class MetricFieldsMappingExtensions
    {
        public static void IncludeBasic(this MetricFields fields)
        {
            fields.Counter[CounterFields.Value] = "val";
            fields.ExcludeCounterValues(CounterFields.Total, CounterFields.MetricSetItemSuffix, CounterFields.SetItemPercent);
            fields.OnlyIncludeMeterValues(MeterFields.Rate1M);
            fields.OnlyIncludeHistogramValues(HistogramFields.P95, HistogramFields.P99);

            // fields.ExcludeHistogramValues();
            // fields.ExcludeCounterValues();
            // fields.ExcludeMeterValues();
            // fields.ExcludeApdexValues();
            // fields.ExcludeGaugeValues();
        }
    }
}