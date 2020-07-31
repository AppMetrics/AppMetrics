// <copyright file="DefaultMetricFieldNames.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics
{
    internal static class DefaultMetricFieldNames
    {
        public static readonly string DefaultMetricsSetItemSuffix = "  items";

        public static IReadOnlyDictionary<ApdexFields, string> Apdex => new Dictionary<ApdexFields, string>
                                                                       {
                                                                           { ApdexFields.Samples, "samples" },
                                                                           { ApdexFields.Score, "score" },
                                                                           { ApdexFields.Satisfied, "satisfied" },
                                                                           { ApdexFields.Tolerating, "tolerating" },
                                                                           { ApdexFields.Frustrating, "frustrating" }
                                                                       };

        public static IReadOnlyDictionary<HistogramFields, string> Histogram => new Dictionary<HistogramFields, string>
                                                                               {
                                                                                   { HistogramFields.Samples, "samples" },
                                                                                   { HistogramFields.LastValue, "last" },
                                                                                   { HistogramFields.Sum, "sum" },
                                                                                   { HistogramFields.Count, "count.hist" },
                                                                                   { HistogramFields.Min, "min" },
                                                                                   { HistogramFields.Max, "max" },
                                                                                   { HistogramFields.Mean, "mean" },
                                                                                   { HistogramFields.Median, "median" },
                                                                                   { HistogramFields.StdDev, "stddev" },
                                                                                   { HistogramFields.P999, "p999" },
                                                                                   { HistogramFields.P99, "p99" },
                                                                                   { HistogramFields.P98, "p98" },
                                                                                   { HistogramFields.P95, "p95" },
                                                                                   { HistogramFields.P75, "p75" },
                                                                                   {
                                                                                       HistogramFields.UserLastValue,
                                                                                       "user.last"
                                                                                   },
                                                                                   { HistogramFields.UserMinValue, "user.min" },
                                                                                   { HistogramFields.UserMaxValue, "user.max" }
                                                                               };

        public static IReadOnlyDictionary<BucketHistogramFields, string> BucketHistogram => new Dictionary<BucketHistogramFields, string>
                                                                               {
                                                                                   { BucketHistogramFields.Sum, "sum" },
                                                                                   { BucketHistogramFields.Count, "count.hist" },
                                                                                   { BucketHistogramFields.Bucket, "bucket" },
                                                                               };

        public static IReadOnlyDictionary<MeterFields, string> Meter => new Dictionary<MeterFields, string>
                                                                       {
                                                                           { MeterFields.Count, "count.meter" },
                                                                           { MeterFields.Rate1M, "rate1m" },
                                                                           { MeterFields.Rate5M, "rate5m" },
                                                                           { MeterFields.Rate15M, "rate15m" },
                                                                           { MeterFields.RateMean, "rate.mean" },
                                                                           { MeterFields.SetItem, DefaultMetricsSetItemSuffix },
                                                                           { MeterFields.SetItemPercent, "percent" }
                                                                       };

        public static IReadOnlyDictionary<CounterFields, string> Counter => new Dictionary<CounterFields, string>
                                                                           {
                                                                               { CounterFields.Total, "total" },
                                                                               { CounterFields.Value, "value" },
                                                                               { CounterFields.SetItem, DefaultMetricsSetItemSuffix },
                                                                               { CounterFields.SetItemPercent, "percent" }
                                                                           };

        public static IReadOnlyDictionary<GaugeFields, string> Gauge => new Dictionary<GaugeFields, string>
                                                                           {
                                                                               { GaugeFields.Value, "value" }
                                                                           };
    }
}
