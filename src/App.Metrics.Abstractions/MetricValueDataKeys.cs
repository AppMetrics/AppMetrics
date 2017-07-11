// <copyright file="MetricValueDataKeys.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics
{
    public class MetricValueDataKeys
    {
        public MetricValueDataKeys()
        {
            Histogram = DataKeyMapping.Histogram;
            Meter = DataKeyMapping.Meter;
            Apdex = DataKeyMapping.Apdex;
            Counter = DataKeyMapping.Counter;
        }

        public MetricValueDataKeys(
            IDictionary<HistogramValueDataKeys, string> histogram = null,
            IDictionary<MeterValueDataKeys, string> meter = null,
            IDictionary<ApdexValueDataKeys, string> apdex = null,
            IDictionary<CounterValueDataKeys, string> counter = null)
        {
            Histogram = DataKeyMapping.Histogram.MergeDifference(histogram);
            Meter = DataKeyMapping.Meter.MergeDifference(meter);
            Apdex = DataKeyMapping.Apdex.MergeDifference(apdex);
            Counter = DataKeyMapping.Counter.MergeDifference(counter);
        }

        public IDictionary<ApdexValueDataKeys, string> Apdex { get; }

        public IDictionary<CounterValueDataKeys, string> Counter { get; }

        public IDictionary<HistogramValueDataKeys, string> Histogram { get; }

        public IDictionary<MeterValueDataKeys, string> Meter { get; }
    }
}