// <copyright file="HumanizeTimerMetricFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;
using App.Metrics.Timer;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeTimerMetricFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is TimerValueSource))
            {
                return arg.ToString();
            }

            var timer = (TimerValueSource)arg;

            var sb = new StringBuilder();

            sb.AppendLine();

            sb.HummanizeTimer(timer);

            return sb.ToString();
        }
    }
}