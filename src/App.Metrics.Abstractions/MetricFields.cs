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

        public void ExcludeApdexValues(params ApdexFields[] keys)
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

        public void ExcludeCounterValues(params CounterFields[] keys)
        {
            if (!keys.Any())
            {
                Counter.Clear();
                return;
            }

            foreach (var key in keys)
            {
                if (Counter.ContainsKey(key))
                {
                    Counter.Remove(key);
                }
            }
        }

        public void ExcludeGaugeValues(params GaugeFields[] keys)
        {
            if (!keys.Any())
            {
                Gauge.Clear();
                return;
            }

            foreach (var key in keys)
            {
                if (Gauge.ContainsKey(key))
                {
                    Gauge.Remove(key);
                }
            }
        }

        public void ExcludeHistogramValues(params HistogramFields[] keys)
        {
            if (!keys.Any())
            {
                Histogram.Clear();
                return;
            }

            foreach (var key in keys)
            {
                if (Histogram.ContainsKey(key))
                {
                    Histogram.Remove(key);
                }
            }
        }

        public void ExcludeMeterValues(params MeterFields[] keys)
        {
            if (!keys.Any())
            {
                Meter.Clear();
                return;
            }

            foreach (var key in keys)
            {
                if (Meter.ContainsKey(key))
                {
                    Meter.Remove(key);
                }
            }
        }

        public void OnlyIncludeApdexValues(params ApdexFields[] keys)
        {
            var apdex = new Dictionary<ApdexFields, string>(Apdex);

            foreach (var key in apdex.Keys)
            {
                if (!keys.Contains(key))
                {
                    Apdex.Remove(key);
                }
            }
        }

        public void OnlyIncludeCounterValues(params CounterFields[] keys)
        {
            var counter = new Dictionary<CounterFields, string>(Counter);

            foreach (var key in counter.Keys)
            {
                if (!keys.Contains(key))
                {
                    Counter.Remove(key);
                }
            }
        }

        public void OnlyIncludeGaugeValues(params GaugeFields[] keys)
        {
            var gauge = new Dictionary<GaugeFields, string>(Gauge);

            foreach (var key in gauge.Keys)
            {
                if (!keys.Contains(key))
                {
                    Gauge.Remove(key);
                }
            }
        }

        public void OnlyIncludeHistogramValues(params HistogramFields[] keys)
        {
            var histogram = new Dictionary<HistogramFields, string>(Histogram);

            foreach (var key in histogram.Keys)
            {
                if (!keys.Contains(key))
                {
                    Histogram.Remove(key);
                }
            }
        }

        public void OnlyIncludeMeterValues(params MeterFields[] keys)
        {
            var meter = new Dictionary<MeterFields, string>(Meter);

            foreach (var key in meter.Keys)
            {
                if (!keys.Contains(key))
                {
                    Meter.Remove(key);
                }
            }
        }
    }
}