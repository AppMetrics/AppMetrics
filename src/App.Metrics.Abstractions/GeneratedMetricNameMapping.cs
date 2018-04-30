// <copyright file="GeneratedMetricNameMapping.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
{
    // TODO: Refactoring AppMetrics/AppMetrics/#251
    public class GeneratedMetricNameMapping
    {
        public GeneratedMetricNameMapping()
        {
            Histogram = DefaultGeneratedMetricNameMapping.Histogram;
            Meter = DefaultGeneratedMetricNameMapping.Meter;
            Apdex = DefaultGeneratedMetricNameMapping.Apdex;
            Counter = DefaultGeneratedMetricNameMapping.Counter;
            Gauge = DefaultGeneratedMetricNameMapping.Gauge;
        }

        public GeneratedMetricNameMapping(
            IDictionary<HistogramValueDataKeys, string> histogram = null,
            IDictionary<MeterValueDataKeys, string> meter = null,
            IDictionary<ApdexValueDataKeys, string> apdex = null,
            IDictionary<CounterValueDataKeys, string> counter = null,
            IDictionary<GaugeValueDataKeys, string> gauge = null)
        {
            Histogram = DefaultGeneratedMetricNameMapping.Histogram.MergeDifference(histogram);
            Meter = DefaultGeneratedMetricNameMapping.Meter.MergeDifference(meter);
            Apdex = DefaultGeneratedMetricNameMapping.Apdex.MergeDifference(apdex);
            Counter = DefaultGeneratedMetricNameMapping.Counter.MergeDifference(counter);
            Gauge = DefaultGeneratedMetricNameMapping.Gauge.MergeDifference(gauge);
        }

        public IDictionary<ApdexValueDataKeys, string> Apdex { get; private set; }

        public IDictionary<CounterValueDataKeys, string> Counter { get; private set; }

        public IDictionary<GaugeValueDataKeys, string> Gauge { get; private set; }

        public IDictionary<HistogramValueDataKeys, string> Histogram { get; private set; }

        public IDictionary<MeterValueDataKeys, string> Meter { get; private set; }

        public void ExcludeApdexValues(params ApdexValueDataKeys[] keys)
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

        public void ExcludeCounterValues(params CounterValueDataKeys[] keys)
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

        public void ExcludeGaugeValues(params GaugeValueDataKeys[] keys)
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

        public void ExcludeHistogramValues(params HistogramValueDataKeys[] keys)
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

        public void ExcludeMeterValues(params MeterValueDataKeys[] keys)
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

        public void OnlyIncludeApdexValues(params ApdexValueDataKeys[] keys)
        {
            var apdex = new Dictionary<ApdexValueDataKeys, string>(Apdex);

            foreach (var key in apdex.Keys)
            {
                if (!keys.Contains(key))
                {
                    Apdex.Remove(key);
                }
            }
        }

        public void OnlyIncludeCounterValues(params CounterValueDataKeys[] keys)
        {
            var counter = new Dictionary<CounterValueDataKeys, string>(Counter);

            foreach (var key in counter.Keys)
            {
                if (!keys.Contains(key))
                {
                    Counter.Remove(key);
                }
            }
        }

        public void OnlyIncludeGaugeValues(params GaugeValueDataKeys[] keys)
        {
            var gauge = new Dictionary<GaugeValueDataKeys, string>(Gauge);

            foreach (var key in gauge.Keys)
            {
                if (!keys.Contains(key))
                {
                    Gauge.Remove(key);
                }
            }
        }

        public void OnlyIncludeHistogramValues(params HistogramValueDataKeys[] keys)
        {
            var histogram = new Dictionary<HistogramValueDataKeys, string>(Histogram);

            foreach (var key in histogram.Keys)
            {
                if (!keys.Contains(key))
                {
                    Histogram.Remove(key);
                }
            }
        }

        public void OnlyIncludeMeterValues(params MeterValueDataKeys[] keys)
        {
            var meter = new Dictionary<MeterValueDataKeys, string>(Meter);

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