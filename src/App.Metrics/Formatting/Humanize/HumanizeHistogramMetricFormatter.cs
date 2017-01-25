// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Text;
using App.Metrics.Histogram;

namespace App.Metrics.Formatting.Humanize
{
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