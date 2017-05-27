// <copyright file="HumanizeHistogramMetricFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;
using App.Metrics.Histogram;

namespace App.Metrics.Formatting.Humanize
{
    // TODO: Remove in 2.0.0
    [Obsolete("Replaced with formatting packages which can be used with the Report Runner")]
    public class HumanizeHistogramMetricFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is HistogramValueSource))
            {
                return arg.ToString();
            }

            var histogram = (HistogramValueSource)arg;

            var sb = new StringBuilder();

            sb.HumanizeHistogram(histogram.Value, histogram.Unit);

            return sb.ToString();
        }
    }
}