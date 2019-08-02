// <copyright file="MetricFields.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
{
    public class MetricFields
    {
        public MetricFields()
        {
            Histogram = DefaultMetricFieldNames.Histogram.ToDictionary(pair => pair.Key, pair => pair.Value);
            BucketHistogram = DefaultMetricFieldNames.BucketHistogram.ToDictionary(pair => pair.Key, pair => pair.Value);
            Meter = DefaultMetricFieldNames.Meter.ToDictionary(pair => pair.Key, pair => pair.Value);
            Apdex = DefaultMetricFieldNames.Apdex.ToDictionary(pair => pair.Key, pair => pair.Value);
            Counter = DefaultMetricFieldNames.Counter.ToDictionary(pair => pair.Key, pair => pair.Value);
            Gauge = DefaultMetricFieldNames.Gauge.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public IDictionary<ApdexFields, string> Apdex { get; }

        public IDictionary<CounterFields, string> Counter { get; }

        public IDictionary<GaugeFields, string> Gauge { get; }

        public IDictionary<HistogramFields, string> Histogram { get; }
        public IDictionary<BucketHistogramFields, string> BucketHistogram { get; }

        public IDictionary<MeterFields, string> Meter { get; }
    }
}