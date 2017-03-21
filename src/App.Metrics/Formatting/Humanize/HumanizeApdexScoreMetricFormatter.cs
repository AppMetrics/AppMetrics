// <copyright file="HumanizeApdexScoreMetricFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;
using App.Metrics.Apdex;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeApdexScoreMetricFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is ApdexValueSource))
            {
                return arg.ToString();
            }

            var apdex = (ApdexValueSource)arg;

            var sb = new StringBuilder();
            sb.HumanizeApdexScore(apdex);
            return sb.ToString();
        }
    }
}