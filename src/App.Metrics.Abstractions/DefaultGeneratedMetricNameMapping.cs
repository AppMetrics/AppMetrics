// <copyright file="DefaultGeneratedMetricNameMapping.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics
{
    internal static class DefaultGeneratedMetricNameMapping
    {
        public static IDictionary<ApdexValueDataKeys, string> Apdex => new Dictionary<ApdexValueDataKeys, string>
                                                                       {
                                                                           { ApdexValueDataKeys.Samples, "samples" },
                                                                           { ApdexValueDataKeys.Score, "score" },
                                                                           { ApdexValueDataKeys.Satisfied, "satisfied" },
                                                                           { ApdexValueDataKeys.Tolerating, "tolerating" },
                                                                           { ApdexValueDataKeys.Frustrating, "frustrating" }
                                                                       };

        public static IDictionary<HistogramValueDataKeys, string> Histogram => new Dictionary<HistogramValueDataKeys, string>
                                                                               {
                                                                                   { HistogramValueDataKeys.Samples, "samples" },
                                                                                   { HistogramValueDataKeys.LastValue, "last" },
                                                                                   { HistogramValueDataKeys.Sum, "sum" },
                                                                                   { HistogramValueDataKeys.Count, "count.hist" },
                                                                                   { HistogramValueDataKeys.Min, "min" },
                                                                                   { HistogramValueDataKeys.Max, "max" },
                                                                                   { HistogramValueDataKeys.Mean, "mean" },
                                                                                   { HistogramValueDataKeys.Median, "median" },
                                                                                   { HistogramValueDataKeys.StdDev, "stddev" },
                                                                                   { HistogramValueDataKeys.P999, "p999" },
                                                                                   { HistogramValueDataKeys.P99, "p99" },
                                                                                   { HistogramValueDataKeys.P98, "p98" },
                                                                                   { HistogramValueDataKeys.P95, "p95" },
                                                                                   { HistogramValueDataKeys.P75, "p75" },
                                                                                   {
                                                                                       HistogramValueDataKeys.UserLastValue,
                                                                                       "user.last"
                                                                                   },
                                                                                   { HistogramValueDataKeys.UserMinValue, "user.min" },
                                                                                   { HistogramValueDataKeys.UserMaxValue, "user.max" }
                                                                               };

        public static IDictionary<MeterValueDataKeys, string> Meter => new Dictionary<MeterValueDataKeys, string>
                                                                       {
                                                                           { MeterValueDataKeys.Count, "count.meter" },
                                                                           { MeterValueDataKeys.Rate1M, "rate1m" },
                                                                           { MeterValueDataKeys.Rate5M, "rate5m" },
                                                                           { MeterValueDataKeys.Rate15M, "rate15m" },
                                                                           { MeterValueDataKeys.RateMean, "rate.mean" },
                                                                           { MeterValueDataKeys.SetItemPercent, "percent" },
                                                                           { MeterValueDataKeys.MetricSetItemSuffix, "  items" }
                                                                       };

        public static IDictionary<CounterValueDataKeys, string> Counter => new Dictionary<CounterValueDataKeys, string>
                                                                           {
                                                                               { CounterValueDataKeys.Total, "total" },
                                                                               { CounterValueDataKeys.SetItemPercent, "percent" },
                                                                               { CounterValueDataKeys.MetricSetItemSuffix, "  items" }
                                                                           };
    }
}
