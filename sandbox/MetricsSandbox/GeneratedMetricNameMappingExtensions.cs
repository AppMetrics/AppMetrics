// <copyright file="GeneratedMetricNameMappingExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics;

namespace MetricsSandbox
{
    public static class GeneratedMetricNameMappingExtensions
    {
        public static void IncludeBasic(this GeneratedMetricNameMapping dataKeys)
        {
            dataKeys.OnlyIncludeApdexValues(ApdexValueDataKeys.Score);
            dataKeys.ExcludeCounterValues(CounterValueDataKeys.Total, CounterValueDataKeys.MetricSetItemSuffix, CounterValueDataKeys.SetItemPercent);
            dataKeys.OnlyIncludeMeterValues(MeterValueDataKeys.Rate1M);
            dataKeys.OnlyIncludeHistogramValues(HistogramValueDataKeys.P95, HistogramValueDataKeys.P99);

            // dataKeys.ExcludeHistogramValues();
            // dataKeys.ExcludeCounterValues();
            // dataKeys.ExcludeMeterValues();
            // dataKeys.ExcludeApdexValues();
            // dataKeys.ExcludeGaugeValues();
        }
    }
}