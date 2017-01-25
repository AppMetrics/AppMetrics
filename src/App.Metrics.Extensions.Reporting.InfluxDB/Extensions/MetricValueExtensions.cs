// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Data;
using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Extensions
{
    internal static class MetricValueExtensions
    {
        public static void AddApdexValues(this ApdexValue apdex, IDictionary<string, object> values)
        {
            values.Add("samples", apdex.SampleSize);
            values.Add("score", apdex.Score);
            values.Add("satisfied", apdex.Satisfied);
            values.Add("tolerating", apdex.Tolerating);
            values.Add("frustrating", apdex.Frustrating);
        }

        public static void AddHistogramValues(this HistogramValue histogram, IDictionary<string, object> values)
        {
            values.Add("samples", histogram.SampleSize);
            values.Add("last", histogram.LastValue);
            values.Add("count.hist", histogram.Count);
            values.Add("min", histogram.Min);
            values.Add("max", histogram.Max);
            values.Add("mean", histogram.Mean);
            values.Add("median", histogram.Median);
            values.Add("stddev", histogram.StdDev);
            values.Add("p999", histogram.Percentile999);
            values.Add("p99", histogram.Percentile99);
            values.Add("p98", histogram.Percentile98);
            values.Add("p95", histogram.Percentile95);
            values.Add("p75", histogram.Percentile75);

            if (histogram.LastUserValue != null)
            {
                values.Add("user.last", histogram.LastUserValue);
            }

            if (histogram.MinUserValue != null)
            {
                values.Add("user.min", histogram.MinUserValue);
            }

            if (histogram.MaxUserValue != null)
            {
                values.Add("user.max", histogram.MaxUserValue);
            }
        }

        public static void AddMeterValues(this MeterValue meter, IDictionary<string, object> values)
        {
            values.Add("count.meter", meter.Count);
            values.Add("rate1m", meter.OneMinuteRate);
            values.Add("rate5m", meter.FiveMinuteRate);
            values.Add("rate15m", meter.FifteenMinuteRate);
            values.Add("rate.mean", meter.MeanRate);
        }
    }
}