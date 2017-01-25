// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Globalization;
using System.Linq;
using System.Text;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Formatting.Humanize
{
    public static class HumanizeMetricValueExtensions
    {
        public static void HumanizeApdexScore(this StringBuilder sb, ApdexValueSource apdex)
        {
            sb.AppendLine();
            sb.AppendLine("Score".FormatReadableMetricValue(apdex.Value.Score.ToString(CultureInfo.CurrentCulture)));
            sb.AppendLine("Sample Size".FormatReadableMetricValue(apdex.Value.SampleSize.ToString(CultureInfo.CurrentCulture)));
            sb.AppendLine("Satisfied".FormatReadableMetricValue(apdex.Value.Satisfied.ToString(CultureInfo.CurrentCulture)));
            sb.AppendLine("Tolerating".FormatReadableMetricValue(apdex.Value.Tolerating.ToString(CultureInfo.CurrentCulture)));
            sb.AppendLine("Frustrating".FormatReadableMetricValue(apdex.Value.Frustrating.ToString(CultureInfo.CurrentCulture)));
        }

        public static void HumanizeCounter(this StringBuilder sb, CounterValueSource counter)
        {
            sb.AppendLine();
            sb.AppendLine("Count".FormatReadableMetricValue(counter.Unit.FormatCount(counter.Value.Count)));

            if (!counter.Value.Items.Any())
            {
                return;
            }

            sb.AppendLine("Total Items".FormatReadableMetricValue(counter.Value.Items.Length.ToString()));

            foreach (var keyValue in counter.Value.Items.Select((x, i) => new { Value = x, Index = i }))
            {
                var key = $"Item {keyValue.Index}";
                var value = $"{keyValue.Value.Percent:00.00}% {keyValue.Value.Count,5} {counter.Unit.Name} [{keyValue.Value.Item}]";
                sb.AppendLine(key.FormatReadableMetricValue(value));
            }
        }

        public static void HumanizeGauge(this StringBuilder sb, GaugeValueSource gauge)
        {
            sb.AppendLine();
            sb.AppendLine("value".FormatReadableMetricValue(gauge.Unit.FormatValue(gauge.Value)));
        }

        public static void HumanizeHistogram(this StringBuilder sb, HistogramValue histogram, Unit unit)
        {
            sb.AppendLine();
            sb.AppendLine("Count".FormatReadableMetricValue(unit.FormatCount(histogram.Count)));
            sb.AppendLine("Last".FormatReadableMetricValue(unit.FormatDuration(histogram.LastValue, null)));

            if (!string.IsNullOrWhiteSpace(histogram.LastUserValue))
            {
                sb.AppendLine("Last User Value".FormatReadableMetricValue(histogram.LastUserValue));
            }

            sb.AppendLine("Min".FormatReadableMetricValue(unit.FormatDuration(histogram.Min, null)));

            if (!string.IsNullOrWhiteSpace(histogram.MinUserValue))
            {
                sb.AppendLine("Min User Value".FormatReadableMetricValue(histogram.MinUserValue));
            }

            sb.AppendLine("Max".FormatReadableMetricValue(unit.FormatDuration(histogram.Max, null)));

            if (!string.IsNullOrWhiteSpace(histogram.MaxUserValue))
            {
                sb.AppendLine("Max User Value".FormatReadableMetricValue(histogram.MaxUserValue));
            }

            sb.AppendLine("Mean".FormatReadableMetricValue(unit.FormatDuration(histogram.Mean, null)));
            sb.AppendLine("StdDev".FormatReadableMetricValue(unit.FormatDuration(histogram.StdDev, null)));
            sb.AppendLine("Median".FormatReadableMetricValue(unit.FormatDuration(histogram.Median, null)));
            sb.AppendLine("75%".FormatReadableMetricValue(unit.FormatDuration(histogram.Percentile75, null), sign: "<="));
            sb.AppendLine("95%".FormatReadableMetricValue(unit.FormatDuration(histogram.Percentile95, null), sign: "<="));
            sb.AppendLine("98%".FormatReadableMetricValue(unit.FormatDuration(histogram.Percentile98, null), sign: "<="));
            sb.AppendLine("99%".FormatReadableMetricValue(unit.FormatDuration(histogram.Percentile99, null), sign: "<="));
            sb.AppendLine("99.9%".FormatReadableMetricValue(unit.FormatDuration(histogram.Percentile999, null), sign: "<="));
        }

        public static void HumanizeMeter(this StringBuilder sb, MeterValue value, Unit unit)
        {
            sb.AppendLine("Count".FormatReadableMetricValue(unit.FormatCount(value.Count)));
            sb.AppendLine("Mean Value".FormatReadableMetricValue(unit.FormatRate(value.MeanRate, value.RateUnit)));
            sb.AppendLine("1 Minute Rate".FormatReadableMetricValue(unit.FormatRate(value.OneMinuteRate, value.RateUnit)));
            sb.AppendLine("5 Minute Rate".FormatReadableMetricValue(unit.FormatRate(value.FiveMinuteRate, value.RateUnit)));
            sb.AppendLine("15 Minute Rate".FormatReadableMetricValue(unit.FormatRate(value.FifteenMinuteRate, value.RateUnit)));

            if (!value.Items.Any())
            {
                return;
            }

            sb.AppendLine("Total Items".FormatReadableMetricValue(value.Items.Length.ToString()));

            foreach (var keyValue in value.Items.Select((x, i) => new { Value = x, Index = i }))
            {
                var key = $"Item {keyValue.Index}";
                var itemValue = $"{keyValue.Value.Percent:00.00}% {keyValue.Value.Value.Count,5} {unit.Name} [{keyValue.Value.Item}]";
                sb.AppendLine(key.FormatReadableMetricValue(itemValue));
                sb.HumanizeMeter(keyValue.Value.Value, unit);
            }
        }

        public static void HummanizeTimer(this StringBuilder sb, TimerValueSource timer)
        {
            sb.AppendLine();
            sb.AppendLine("Active Sessions".FormatReadableMetricValue(timer.Value.ActiveSessions.ToString()));
            sb.AppendLine("Total Time".FormatReadableMetricValue(timer.Unit.FormatDuration(timer.Value.TotalTime, timer.DurationUnit)));

            sb.HumanizeMeter(timer.Value.Rate, timer.Unit);
            sb.HumanizeHistogram(timer.Value.Histogram, timer.Unit);
        }
    }
}