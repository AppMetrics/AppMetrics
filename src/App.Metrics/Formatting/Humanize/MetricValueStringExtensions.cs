// <copyright file="MetricValueStringExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Formatting.Humanize
{
    public static class MetricValueStringExtensions
    {
        private const int Padding = 20;

        public static string FormatReadableMetricValue(this string label, string value, string sign = "=")
        {
            var pad = string.Empty;

            if (label.Length + 2 + sign.Length < Padding)
            {
                pad = new string(' ', Padding - label.Length - 1 - sign.Length);
            }

            return $"{pad}{label} {sign} {value}";
        }
    }
}