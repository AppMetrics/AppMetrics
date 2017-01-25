// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Text;
using App.Metrics.Meter;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeMeterMetricFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is MeterValueSource))
            {
                return arg.ToString();
            }

            var meter = (MeterValueSource)arg;

            var sb = new StringBuilder();
            WriteMeter(sb, meter.Value, meter.Unit, meter.RateUnit);
            return sb.ToString();
        }

        private static void WriteMeter(StringBuilder sb, MeterValue meter, Unit unit, TimeUnit rateUnit)
        {
            sb.AppendLine();
            sb.AppendLine("Count".FormatReadableMetricValue(unit.FormatCount(meter.Count)));
            sb.AppendLine("Mean Value".FormatReadableMetricValue(unit.FormatRate(meter.MeanRate, rateUnit)));
            sb.AppendLine("1 Minute Rate".FormatReadableMetricValue(unit.FormatRate(meter.OneMinuteRate, rateUnit)));
            sb.AppendLine("5 Minute Rate".FormatReadableMetricValue(unit.FormatRate(meter.FiveMinuteRate, rateUnit)));
            sb.AppendLine("15 Minute Rate".FormatReadableMetricValue(unit.FormatRate(meter.FifteenMinuteRate, rateUnit)));
        }
    }
}