// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Data;
using App.Metrics.Data.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal
{
    internal sealed class DefaultMetricContextRegistry : IMetricContextRegistry
    {
        private readonly MetricMetaCatalog<IApdex, ApdexValueSource, ApdexValue> _apdexScores =
            new MetricMetaCatalog<IApdex, ApdexValueSource, ApdexValue>();

        private readonly MetricMetaCatalog<ICounter, CounterValueSource, CounterValue> _counters =
            new MetricMetaCatalog<ICounter, CounterValueSource, CounterValue>();

        private readonly MetricMetaCatalog<IMetricValueProvider<double>, GaugeValueSource, double> _gauges =
            new MetricMetaCatalog<IMetricValueProvider<double>, GaugeValueSource, double>();

        private readonly GlobalMetricTags _globalTags;

        private readonly MetricMetaCatalog<IHistogram, HistogramValueSource, HistogramValue> _histograms =
            new MetricMetaCatalog<IHistogram, HistogramValueSource, HistogramValue>();

        private readonly MetricMetaCatalog<IMeter, MeterValueSource, MeterValue> _meters =
            new MetricMetaCatalog<IMeter, MeterValueSource, MeterValue>();

        private readonly MetricMetaCatalog<ITimer, TimerValueSource, TimerValue> _timers =
            new MetricMetaCatalog<ITimer, TimerValueSource, TimerValue>();

        public DefaultMetricContextRegistry(string context)
            : this(context, new GlobalMetricTags())
        {
        }

        public DefaultMetricContextRegistry(string context, GlobalMetricTags globalTags)
        {
            if (globalTags == null)
            {
                throw new ArgumentNullException(nameof(globalTags));
            }

            if (context.IsMissing())
            {
                throw new ArgumentException("Registry Context cannot be null or empty", nameof(context));
            }

            _globalTags = globalTags;
            Context = context;

            DataProvider = new DefaultMetricRegistryManager(
                () => _gauges.All,
                () => _counters.All,
                () => _meters.All,
                () => _histograms.All,
                () => _timers.All,
                () => _apdexScores.All);
        }

        public string Context { get; }

        public IMetricRegistryManager DataProvider { get; }

        public IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric
        {
            return _apdexScores.GetOrAdd(
                options.Name,
                () =>
                {
                    var apdex = builder();
                    var valueSource = new ApdexValueSource(options.Name, apdex, AllTags(options.Tags), options.ResetOnReporting);
                    return Tuple.Create((IApdex)apdex, valueSource);
                });
        }

        public void ClearAllMetrics()
        {
            _gauges.Clear();
            _counters.Clear();
            _meters.Clear();
            _histograms.Clear();
            _timers.Clear();
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric
        {
            return _counters.GetOrAdd(
                options.Name,
                () =>
                {
                    var counter = builder();
                    var valueSource = new CounterValueSource(
                        options.Name,
                        counter,
                        options.MeasurementUnit,
                        AllTags(options.Tags),
                        options.ResetOnReporting,
                        options.ReportItemPercentages,
                        options.ReportSetItems);
                    return Tuple.Create((ICounter)counter, valueSource);
                });
        }

        public void Gauge(GaugeOptions options, Func<IMetricValueProvider<double>> valueProvider)
        {
            _gauges.GetOrAdd(
                options.Name,
                () =>
                {
                    var gauge = valueProvider();
                    var valueSource = new GaugeValueSource(options.Name, gauge, options.MeasurementUnit, AllTags(options.Tags));
                    return Tuple.Create(gauge, valueSource);
                });
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric
        {
            return _histograms.GetOrAdd(
                options.Name,
                () =>
                {
                    var histogram = builder();
                    var valueSource = new HistogramValueSource(options.Name, histogram, options.MeasurementUnit, AllTags(options.Tags));
                    return Tuple.Create((IHistogram)histogram, valueSource);
                });
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric
        {
            return _meters.GetOrAdd(
                options.Name,
                () =>
                {
                    var meter = builder();
                    var valueSource = new MeterValueSource(options.Name, meter, options.MeasurementUnit, options.RateUnit, AllTags(options.Tags));
                    return Tuple.Create((IMeter)meter, valueSource);
                });
        }

        public void ResetMetricsValues()
        {
            _gauges.Reset();
            _counters.Reset();
            _meters.Reset();
            _histograms.Reset();
            _timers.Reset();
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric
        {
            return _timers.GetOrAdd(
                options.Name,
                () =>
                {
                    var timer = builder();
                    var valueSource = new TimerValueSource(
                        options.Name,
                        timer,
                        options.MeasurementUnit,
                        options.RateUnit,
                        options.DurationUnit,
                        AllTags(options.Tags));
                    return Tuple.Create((ITimer)timer, valueSource);
                });
        }

        private MetricTags AllTags(MetricTags metricTags)
        {
            return new MetricTags(_globalTags.Concat(metricTags).ToDictionary(t => t.Key, t => t.Value));
        }

        private class MetricMetaCatalog<TMetric, TValue, TMetricValue>
            where TValue : MetricValueSource<TMetricValue>
        {
            private readonly ConcurrentDictionary<string, MetricMeta> _metrics =
                new ConcurrentDictionary<string, MetricMeta>();

            public IEnumerable<TValue> All
            {
                get { return _metrics.Values.OrderBy(m => m.Name).Select(v => v.Value); }
            }

            public void Clear()
            {
                var values = _metrics.Values;

                _metrics.Clear();

                foreach (var value in values)
                {
                    using (value.Metric as IDisposable)
                    {
                    }
                }
            }

            public TMetric GetOrAdd(string name, Func<Tuple<TMetric, TValue>> metricProvider)
            {
                return _metrics.GetOrAdd(
                    name,
                    n =>
                    {
                        var result = metricProvider();
                        return new MetricMeta(result.Item1, result.Item2);
                    }).Metric;
            }

            public void Reset()
            {
                foreach (var metric in _metrics.Values)
                {
                    var resetable = metric.Metric as IResetableMetric;
                    resetable?.Reset();
                }
            }

            private class MetricMeta
            {
                public MetricMeta(TMetric metric, TValue valueUnit)
                {
                    Metric = metric;
                    Value = valueUnit;
                }

                public TMetric Metric { get; }

                public string Name => Value.Name;

                public TValue Value { get; }
            }
        }
    }
}