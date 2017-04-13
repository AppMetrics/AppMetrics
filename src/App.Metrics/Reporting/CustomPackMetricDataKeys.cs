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
            Counter = new Dictionary<CounterValueDataKeys, string>();
        }

        public CustomPackMetricDataKeys(
            Dictionary<HistogramDataKeys, string> histogram = null,
            Dictionary<MeterValueDataKeys, string> meter = null,
            Dictionary<ApdexValueDataKeys, string> apdex = null,
            Dictionary<CounterValueDataKeys, string> counter = null)
        {
            Histogram = histogram ?? new Dictionary<HistogramDataKeys, string>();
            Meter = meter ?? new Dictionary<MeterValueDataKeys, string>();
            Apdex = apdex ?? new Dictionary<ApdexValueDataKeys, string>();
            Counter = counter ?? new Dictionary<CounterValueDataKeys, string>();
        }

        public Dictionary<ApdexValueDataKeys, string> Apdex { get; }

        public Dictionary<CounterValueDataKeys, string> Counter { get; }

        public Dictionary<HistogramDataKeys, string> Histogram { get; }

        public Dictionary<MeterValueDataKeys, string> Meter { get; }
    }
}