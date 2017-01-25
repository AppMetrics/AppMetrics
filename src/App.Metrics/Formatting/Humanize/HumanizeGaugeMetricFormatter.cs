// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Text;
using App.Metrics.Gauge;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeGaugeMetricFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is GaugeValueSource))
            {
                return arg.ToString();
            }

            var gauge = (GaugeValueSource)arg;

            var sb = new StringBuilder();

            sb.HumanizeGauge(gauge);

            return sb.ToString();
        }
    }
}