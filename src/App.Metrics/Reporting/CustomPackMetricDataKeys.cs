// <copyright file="CustomPackMetricDataKeys.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Reporting
{
    public class CustomPackMetricDataKeys
    {
        public CustomPackMetricDataKeys()
        {
            Histogram = new Dictionary<HistogramDataKeys, string>();
            Meter = new Dictionary<MeterValueDataKeys, string>();
            Apdex = new Dictionary<ApdexValueDataKeys, string>();
        }

        public CustomPackMetricDataKeys(
            Dictionary<HistogramDataKeys, string> histogram,
            Dictionary<MeterValueDataKeys, string> meter,
            Dictionary<ApdexValueDataKeys, string> apdex)
        {
            Histogram = histogram;
            Meter = meter;
            Apdex = apdex;
        }

        public Dictionary<ApdexValueDataKeys, string> Apdex { get; }

        public Dictionary<HistogramDataKeys, string> Histogram { get; }

        public Dictionary<MeterValueDataKeys, string> Meter { get; }
    }
}