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
            fields.ExcludeCounterFields(CounterFields.Total, CounterFields.SetItem, CounterFields.SetItemPercent);
            fields.OnlyIncludeMeterFields(MeterFields.Rate1M);
            fields.OnlyIncludeHistogramFields(HistogramFields.P95, HistogramFields.P99);

            // fields.ExcludeHistogramFields();
            // fields.ExcludeCounterFields();
            // fields.ExcludeMeterFields();
            // fields.ExcludeApdexFields();
            // fields.ExcludeGaugeFields();
        }
    }
}