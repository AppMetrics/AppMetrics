// <copyright file="DefaultMetricContextRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Counter;
using App.Metrics.Counter.Abstractions;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Abstractions;
using App.Metrics.Meter;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;

namespace App.Metrics.Registry.Internal
{
    internal sealed class DefaultMetricContextRegistry : IMetricContextRegistry
    {
        private readonly MetricMetaCatalog<IApdex, ApdexValueSource, ApdexValue> _apdexScores =
            new MetricMetaCatalog<IApdex, ApdexValueSource, ApdexValue>();

        private readonly MetricMetaCatalog<ICounter, CounterValueSource, CounterValue> _counters =
            new MetricMetaCatalog<ICounter, CounterValueSource, CounterValue>();

        private readonly MetricMetaCatalog<IGauge, GaugeValueSource, double> _gauges =
            new MetricMetaCatalog<IGauge, GaugeValueSource, double>();

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
            _globalTags = globalTags ?? throw new ArgumentNullException(nameof(globalTags));

            if (context.IsMissing())
            {
                throw new ArgumentException("Registry Context cannot be null or empty", nameof(context));
            }

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
                    var valueSource = new ApdexValueSource(
                        options.Name,
                        apdex,
                        AllTags(options.Tags),
                        options.ResetOnReporting);
                    return Tuple.Create((IApdex)apdex, valueSource);
                });
        }

        /// <inheritdoc />
        public IApdex Apdex<T>(ApdexOptions options, MetricTags tags, Func<T> builder)
            where T : IApdexMetric
        {
            var metricName = tags.AsMetricName(options.Name);

            return _apdexScores.GetOrAdd(
                metricName,
                () =>
                {
                    var apdex = builder();
                    var valueSource = new ApdexValueSource(
                        metricName,
                        apdex,
                        AllTags(MetricTags.Concat(options.Tags, tags)),
                        options.ResetOnReporting);
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

        /// <inheritdoc />
        public ICounter Counter<T>(CounterOptions options, MetricTags tags, Func<T> builder)
            where T : ICounterMetric
        {
            var metricName = tags.AsMetricName(options.Name);

            return _counters.GetOrAdd(
                metricName,
                () =>
                {
                    var counter = builder();
                    var valueSource = new CounterValueSource(
                        metricName,
                        counter,
                        options.MeasurementUnit,
                        AllTags(MetricTags.Concat(options.Tags, tags)),
                        options.ResetOnReporting,
                        options.ReportItemPercentages,
                        options.ReportSetItems);
                    return Tuple.Create((ICounter)counter, valueSource);
                });
        }

        public IGauge Gauge<T>(GaugeOptions options, Func<T> builder)
            where T : IGaugeMetric
        {
            return _gauges.GetOrAdd(
                options.Name,
                () =>
                {
                    var gauge = builder();
                    var valueSource = new GaugeValueSource(
                        options.Name,
                        gauge,
                        options.MeasurementUnit,
                        AllTags(options.Tags));
                    return Tuple.Create((IGauge)gauge, valueSource);
                });
        }

        /// <inheritdoc />
        public IGauge Gauge<T>(GaugeOptions options, MetricTags tags, Func<T> builder)
            where T : IGaugeMetric
        {
            var metricName = tags.AsMetricName(options.Name);

            return _gauges.GetOrAdd(
                metricName,
                () =>
                {
                    var gauge = builder();
                    var valueSource = new GaugeValueSource(
                        metricName,
                        gauge,
                        options.MeasurementUnit,
                        AllTags(MetricTags.Concat(options.Tags, tags)));

                    return Tuple.Create((IGauge)gauge, valueSource);
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
                    var valueSource = new HistogramValueSource(
                        options.Name,
                        histogram,
                        options.MeasurementUnit,
                        AllTags(options.Tags));
                    return Tuple.Create((IHistogram)histogram, valueSource);
                });
        }

        /// <inheritdoc />
        public IHistogram Histogram<T>(HistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IHistogramMetric
        {
            var metricName = tags.AsMetricName(options.Name);

            return _histograms.GetOrAdd(
                metricName,
                () =>
                {
                    var histogram = builder();
                    var valueSource = new HistogramValueSource(
                        metricName,
                        histogram,
                        options.MeasurementUnit,
                        AllTags(MetricTags.Concat(options.Tags, tags)));
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
                    var valueSource = new MeterValueSource(
                        options.Name,
                        meter,
                        options.MeasurementUnit,
                        options.RateUnit,
                        AllTags(options.Tags));
                    return Tuple.Create((IMeter)meter, valueSource);
                });
        }

        /// <inheritdoc />
        public IMeter Meter<T>(MeterOptions options, MetricTags tags, Func<T> builder)
            where T : IMeterMetric
        {
            var metricName = tags.AsMetricName(options.Name);

            return _meters.GetOrAdd(
                metricName,
                () =>
                {
                    var meter = builder();
                    var valueSource = new MeterValueSource(
                        metricName,
                        meter,
                        options.MeasurementUnit,
                        options.RateUnit,
                        AllTags(MetricTags.Concat(options.Tags, tags)));
                    return Tuple.Create((IMeter)meter, valueSource);
                });
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

        /// <inheritdoc />
        public ITimer Timer<T>(TimerOptions options, MetricTags tags, Func<T> builder)
            where T : ITimerMetric
        {
            var metricName = tags.AsMetricName(options.Name);

            return _timers.GetOrAdd(
                metricName,
                () =>
                {
                    var timer = builder();
                    var valueSource = new TimerValueSource(
                        metricName,
                        timer,
                        options.MeasurementUnit,
                        options.RateUnit,
                        options.DurationUnit,
                        AllTags(MetricTags.Concat(options.Tags, tags)));
                    return Tuple.Create((ITimer)timer, valueSource);
                });
        }

        private MetricTags AllTags(MetricTags metricTags) { return MetricTags.Concat(metricTags, _globalTags); }

        private sealed class MetricMetaCatalog<TMetric, TValue, TMetricValue>
            where TValue : MetricValueSourceBase<TMetricValue>
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
                                    }).
                                Metric;
            }

            private sealed class MetricMeta
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