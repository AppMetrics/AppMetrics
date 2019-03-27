// <copyright file="GraphiteMetricFieldExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class GraphiteMetricFieldExtensions
    {
        public static void DefaultGraphiteMetricFieldNames(this MetricFields fields)
        {
            fields.Apdex.Set(ApdexFields.Frustrating, "Frustrating");
            fields.Apdex.Set(ApdexFields.Samples, "Samples");
            fields.Apdex.Set(ApdexFields.Score, "Score");
            fields.Apdex.Set(ApdexFields.Tolerating, "Tolerating");
            fields.Apdex.Set(ApdexFields.Satisfied, "Satisfied");

            fields.Counter.Set(CounterFields.Total, "Total");
            fields.Counter.Set(CounterFields.Value, "Value");
            fields.Counter.Set(CounterFields.SetItem, "-SetItem");
            fields.Counter.Set(CounterFields.SetItemPercent, "Percent");

            fields.Gauge.Set(GaugeFields.Value, "Value");

            fields.Histogram.Set(HistogramFields.Count, "Count");
            fields.Histogram.Set(HistogramFields.UserLastValue, "User-Last");
            fields.Histogram.Set(HistogramFields.UserMinValue, "User-Min");
            fields.Histogram.Set(HistogramFields.UserMaxValue, "User-Max");
            fields.Histogram.Set(HistogramFields.LastValue, "Last");
            fields.Histogram.Set(HistogramFields.Samples, "Samples");
            fields.Histogram.Set(HistogramFields.Min, "Min");
            fields.Histogram.Set(HistogramFields.Max, "Max");
            fields.Histogram.Set(HistogramFields.Mean, "Mean");
            fields.Histogram.Set(HistogramFields.Median, "Median");
            fields.Histogram.Set(HistogramFields.P75, "Percentile-75");
            fields.Histogram.Set(HistogramFields.P95, "Percentile-95");
            fields.Histogram.Set(HistogramFields.P98, "Percentile-98");
            fields.Histogram.Set(HistogramFields.P99, "Percentile-99");
            fields.Histogram.Set(HistogramFields.P999, "Percentile-999");
            fields.Histogram.Set(HistogramFields.Samples, "Samples");
            fields.Histogram.Set(HistogramFields.StdDev, "StdDev");
            fields.Histogram.Set(HistogramFields.Sum, "Sum");

            fields.Meter.Set(MeterFields.Count, "Total");
            fields.Meter.Set(MeterFields.RateMean, "Rate-Mean");
            fields.Meter.Set(MeterFields.SetItem, "-SetItem");
            fields.Meter.Set(MeterFields.SetItemPercent, "Percent");
            fields.Meter.Set(MeterFields.Rate1M, "Rate-1-Min");
            fields.Meter.Set(MeterFields.Rate5M, "Rate-5-Min");
            fields.Meter.Set(MeterFields.Rate15M, "Rate-15-Min");
        }
    }
}
