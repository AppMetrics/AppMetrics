// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Text;
using App.Metrics.Counter;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeCounterMetricFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return string.Empty;
            }

            if (!(arg is CounterValueSource))
            {
                return arg.ToString();
            }

            var counter = (CounterValueSource)arg;

            var sb = new StringBuilder();
            sb.HumanizeCounter(counter);
            return sb.ToString();
        }
    }
}