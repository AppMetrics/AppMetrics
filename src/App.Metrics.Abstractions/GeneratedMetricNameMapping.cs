// <copyright file="GeneratedMetricNameMapping.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics
{
    public class GeneratedMetricNameMapping
    {
        public GeneratedMetricNameMapping()
        {
            Histogram = DefaultGeneratedMetricNameMapping.Histogram;
            Meter = DefaultGeneratedMetricNameMapping.Meter;
            Apdex = DefaultGeneratedMetricNameMapping.Apdex;
            Counter = DefaultGeneratedMetricNameMapping.Counter;
        }

        public GeneratedMetricNameMapping(
            IDictionary<HistogramValueDataKeys, string> histogram = null,
            IDictionary<MeterValueDataKeys, string> meter = null,
            IDictionary<ApdexValueDataKeys, string> apdex = null,
            IDictionary<CounterValueDataKeys, string> counter = null)
        {
            Histogram = DefaultGeneratedMetricNameMapping.Histogram.MergeDifference(histogram);
            Meter = DefaultGeneratedMetricNameMapping.Meter.MergeDifference(meter);
            Apdex = DefaultGeneratedMetricNameMapping.Apdex.MergeDifference(apdex);
            Counter = DefaultGeneratedMetricNameMapping.Counter.MergeDifference(counter);
        }

        public IDictionary<ApdexValueDataKeys, string> Apdex { get; }

        public IDictionary<CounterValueDataKeys, string> Counter { get; }

        public IDictionary<HistogramValueDataKeys, string> Histogram { get; }

        public IDictionary<MeterValueDataKeys, string> Meter { get; }
    }
}