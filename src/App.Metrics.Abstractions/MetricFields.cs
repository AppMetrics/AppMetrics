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
            Histogram = DefaultMetricFieldNames.Histogram;
            Meter = DefaultMetricFieldNames.Meter;
            Apdex = DefaultMetricFieldNames.Apdex;
            Counter = DefaultMetricFieldNames.Counter;
            Gauge = DefaultMetricFieldNames.Gauge;
        }

        public MetricFields(
            IDictionary<HistogramFields, string> histogram = null,
            IDictionary<MeterFields, string> meter = null,
            IDictionary<ApdexFields, string> apdex = null,
            IDictionary<CounterFields, string> counter = null,
            IDictionary<GaugeFields, string> gauge = null)
        {
            Histogram = DefaultMetricFieldNames.Histogram.MergeDifference(histogram);
            Meter = DefaultMetricFieldNames.Meter.MergeDifference(meter);
            Apdex = DefaultMetricFieldNames.Apdex.MergeDifference(apdex);
            Counter = DefaultMetricFieldNames.Counter.MergeDifference(counter);
            Gauge = DefaultMetricFieldNames.Gauge.MergeDifference(gauge);
        }

        public IDictionary<ApdexFields, string> Apdex { get; private set; }

        public IDictionary<CounterFields, string> Counter { get; private set; }

        public IDictionary<GaugeFields, string> Gauge { get; private set; }

        public IDictionary<HistogramFields, string> Histogram { get; private set; }

        public IDictionary<MeterFields, string> Meter { get; private set; }

        public void ExcludeApdexFields(params ApdexFields[] keys)
        {
            if (!keys.Any())
            {
                Apdex.Clear();
                return;
            }

            foreach (var key in keys)
            {
                if (Apdex.ContainsKey(key))
                {
                    Apdex.Remove(key);
                }
            }
        }

        public void ExcludeCounterFields(params CounterFields[] fields)
        {
            if (!fields.Any())
            {
                Counter.Clear();
                return;
            }

            foreach (var key in fields)
            {
                if (Counter.ContainsKey(key))
                {
                    Counter.Remove(key);
                }
            }
        }

        public void ExcludeGaugeFields(params GaugeFields[] fields)
        {
            if (!fields.Any())
            {
                Gauge.Clear();
                return;
            }

            foreach (var key in fields)
            {
                if (Gauge.ContainsKey(key))
                {
                    Gauge.Remove(key);
                }
            }
        }

        public void ExcludeHistogramFields(params HistogramFields[] fields)
        {
            if (!fields.Any())
            {
                Histogram.Clear();
                return;
            }

            foreach (var key in fields)
            {
                if (Histogram.ContainsKey(key))
                {
                    Histogram.Remove(key);
                }
            }
        }

        public void ExcludeMeterFields(params MeterFields[] fields)
        {
            if (!fields.Any())
            {
                Meter.Clear();
                return;
            }

            foreach (var key in fields)
            {
                if (Meter.ContainsKey(key))
                {
                    Meter.Remove(key);
                }
            }
        }

        public void OnlyIncludeApdexFields(params ApdexFields[] fields)
        {
            var apdex = new Dictionary<ApdexFields, string>(Apdex);

            foreach (var key in apdex.Keys)
            {
                if (!fields.Contains(key))
                {
                    Apdex.Remove(key);
                }
            }
        }

        public void OnlyIncludeCounterFields(params CounterFields[] fields)
        {
            var counter = new Dictionary<CounterFields, string>(Counter);

            foreach (var key in counter.Keys)
            {
                if (!fields.Contains(key))
                {
                    Counter.Remove(key);
                }
            }
        }

        public void OnlyIncludeGaugeFields(params GaugeFields[] fields)
        {
            var gauge = new Dictionary<GaugeFields, string>(Gauge);

            foreach (var key in gauge.Keys)
            {
                if (!fields.Contains(key))
                {
                    Gauge.Remove(key);
                }
            }
        }

        public void OnlyIncludeHistogramFields(params HistogramFields[] fields)
        {
            var histogram = new Dictionary<HistogramFields, string>(Histogram);

            foreach (var key in histogram.Keys)
            {
                if (!fields.Contains(key))
                {
                    Histogram.Remove(key);
                }
            }
        }

        public void OnlyIncludeMeterFields(params MeterFields[] fields)
        {
            var meter = new Dictionary<MeterFields, string>(Meter);

            foreach (var key in meter.Keys)
            {
                if (!fields.Contains(key))
                {
                    Meter.Remove(key);
                }
            }
        }
    }
}