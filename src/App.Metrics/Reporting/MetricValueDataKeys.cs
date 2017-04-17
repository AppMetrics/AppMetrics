// <copyright file="MetricValueDataKeys.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Core.Internal;

namespace App.Metrics.Reporting
{
    public class MetricValueDataKeys
    {
        public MetricValueDataKeys()
        {
            Histogram = Constants.DataKeyMapping.Histogram;
            Meter = Constants.DataKeyMapping.Meter;
            Apdex = Constants.DataKeyMapping.Apdex;
            Counter = Constants.DataKeyMapping.Counter;
        }

        public MetricValueDataKeys(
            IDictionary<HistogramValueDataKeys, string> histogram = null,
            IDictionary<MeterValueDataKeys, string> meter = null,
            IDictionary<ApdexValueDataKeys, string> apdex = null,
            IDictionary<CounterValueDataKeys, string> counter = null)
        {
            Histogram = Constants.DataKeyMapping.Histogram.MergeDifference(histogram);
            Meter = Constants.DataKeyMapping.Meter.MergeDifference(meter);
            Apdex = Constants.DataKeyMapping.Apdex.MergeDifference(apdex);
            Counter = Constants.DataKeyMapping.Counter.MergeDifference(counter);
        }

        public IDictionary<ApdexValueDataKeys, string> Apdex { get; }

        public IDictionary<CounterValueDataKeys, string> Counter { get; }

        public IDictionary<HistogramValueDataKeys, string> Histogram { get; }

        public IDictionary<MeterValueDataKeys, string> Meter { get; }
    }
}