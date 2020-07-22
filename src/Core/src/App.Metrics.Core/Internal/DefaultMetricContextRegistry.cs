// <copyright file="DefaultMetricContextRegistry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Logging;
using App.Metrics.Meter;
using App.Metrics.Registry;
using App.Metrics.Timer;

namespace App.Metrics.Internal
{
    public sealed class DefaultMetricContextRegistry : IMetricContextRegistry
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsRegistry>();

        private readonly MetricMetaCatalog<IApdex, ApdexValueSource, ApdexValue> _apdexScores =
            new MetricMetaCatalog<IApdex, ApdexValueSource, ApdexValue>();

        private readonly MetricMetaCatalog<ICounter, CounterValueSource, CounterValue> _counters =
            new MetricMetaCatalog<ICounter, CounterValueSource, CounterValue>();

        private readonly MetricMetaCatalog<IGauge, GaugeValueSource, double> _gauges =
            new MetricMetaCatalog<IGauge, GaugeValueSource, double>();

        private readonly GlobalMetricTags _globalTags;

        private readonly ContextualMetricTagProviders _contextualTags;

        private readonly MetricMetaCatalog<IHistogram, HistogramValueSource, HistogramValue> _histograms =
            new MetricMetaCatalog<IHistogram, HistogramValueSource, HistogramValue>();

        private readonly MetricMetaCatalog<IBucketHistogram, BucketHistogramValueSource, BucketHistogramValue> _bucketHistograms =
            new MetricMetaCatalog<IBucketHistogram, BucketHistogramValueSource, BucketHistogramValue>();

        private readonly MetricMetaCatalog<IMeter, MeterValueSource, MeterValue> _meters =
            new MetricMetaCatalog<IMeter, MeterValueSource, MeterValue>();

        private readonly MetricMetaCatalog<ITimer, TimerValueSource, TimerValue> _timers =
            new MetricMetaCatalog<ITimer, TimerValueSource, TimerValue>();

        private readonly MetricMetaCatalog<ITimer, BucketTimerValueSource, BucketTimerValue> _bucketTimers =
            new MetricMetaCatalog<ITimer, BucketTimerValueSource, BucketTimerValue>();

        public DefaultMetricContextRegistry(string context)
            : this(context, new GlobalMetricTags(), new ContextualMetricTagProviders())
        {
        }

        public DefaultMetricContextRegistry(string context, GlobalMetricTags globalTags, ContextualMetricTagProviders contextualTags)
        {
            _globalTags = globalTags ?? throw new ArgumentNullException(nameof(globalTags));
            _contextualTags = contextualTags ?? throw new ArgumentNullException(nameof(contextualTags));

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
                () => _bucketHistograms.All,
                () => _timers.All,
                () => _bucketTimers.All,
                () => _apdexScores.All);
        }

        public string Context { get; }

        public IMetricRegistryManager DataProvider { get; }

        public IApdex Apdex<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _apdexScores.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Apdex {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var apdex = builder();
                    var valueSource = new ApdexValueSource(
                        metricName,
                        apdex,
                        allTags,
                        options.ResetOnReporting);
                    return Tuple.Create((IApdex)apdex, valueSource);
                });
        }

        /// <inheritdoc />
        public IApdex Apdex<T>(ApdexOptions options, MetricTags tags, Func<T> builder)
            where T : IApdexMetric
        {
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _apdexScores.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Apdex {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var apdex = builder();
                    var valueSource = new ApdexValueSource(
                        metricName,
                        apdex,
                        allTags,
                        options.ResetOnReporting);
                    return Tuple.Create((IApdex)apdex, valueSource);
                });
        }

        public void ClearAllMetrics()
        {
            Logger.Trace("Clearing all metrics");

            _gauges.Clear();
            _counters.Clear();
            _meters.Clear();
            _histograms.Clear();
            _timers.Clear();

            Logger.Trace("Cleared all metrics");
        }

        public ICounter Counter<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _counters.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Counter {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var counter = builder();
                    var valueSource = new CounterValueSource(
                        metricName,
                        counter,
                        options.MeasurementUnit,
                        allTags,
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
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _counters.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Counter {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var counter = builder();
                    var valueSource = new CounterValueSource(
                        metricName,
                        counter,
                        options.MeasurementUnit,
                        allTags,
                        options.ResetOnReporting,
                        options.ReportItemPercentages,
                        options.ReportSetItems);
                    return Tuple.Create((ICounter)counter, valueSource);
                });
        }

        public IGauge Gauge<T>(GaugeOptions options, Func<T> builder)
            where T : IGaugeMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _gauges.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Gauge {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var gauge = builder();
                    var valueSource = new GaugeValueSource(
                        metricName,
                        gauge,
                        options.MeasurementUnit,
                        allTags,
                        options.ResetOnReporting);
                    return Tuple.Create((IGauge)gauge, valueSource);
                });
        }

        /// <inheritdoc />
        public IGauge Gauge<T>(GaugeOptions options, MetricTags tags, Func<T> builder)
            where T : IGaugeMetric
        {
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _gauges.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Gauge {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags);

                    var gauge = builder();
                    var valueSource = new GaugeValueSource(
                        metricName,
                        gauge,
                        options.MeasurementUnit,
                        allTags,
                        options.ResetOnReporting);

                    return Tuple.Create((IGauge)gauge, valueSource);
                });
        }

        public IHistogram Histogram<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _histograms.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Histogram {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var histogram = builder();
                    var valueSource = new HistogramValueSource(
                        metricName,
                        histogram,
                        options.MeasurementUnit,
                        allTags,
                        options.ResetOnReporting);
                    return Tuple.Create((IHistogram)histogram, valueSource);
                });
        }

        /// <inheritdoc />
        public IHistogram Histogram<T>(HistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IHistogramMetric
        {
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _histograms.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Histogram {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var histogram = builder();
                    var valueSource = new HistogramValueSource(
                        metricName,
                        histogram,
                        options.MeasurementUnit,
                        allTags,
                        options.ResetOnReporting);
                    return Tuple.Create((IHistogram)histogram, valueSource);
                });
        }

        public IBucketHistogram BucketHistogram<T>(BucketHistogramOptions options, Func<T> builder)
            where T : IBucketHistogramMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _bucketHistograms.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Bucket Histogram {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var histogram = builder();
                    var valueSource = new BucketHistogramValueSource(
                        metricName,
                        histogram,
                        options.MeasurementUnit,
                        allTags);
                    return Tuple.Create((IBucketHistogram)histogram, valueSource);
                });
        }

        /// <inheritdoc />
        public IBucketHistogram BucketHistogram<T>(BucketHistogramOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketHistogramMetric
        {
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _bucketHistograms.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Bucket Histogram {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var histogram = builder();
                    var valueSource = new BucketHistogramValueSource(
                        metricName,
                        histogram,
                        options.MeasurementUnit,
                        allTags);
                    return Tuple.Create((IBucketHistogram)histogram, valueSource);
                });
        }

        public IMeter Meter<T>(MeterOptions options, Func<T> builder)
            where T : IMeterMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _meters.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Meter {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var meter = builder();
                    var valueSource = new MeterValueSource(
                        metricName,
                        meter,
                        options.MeasurementUnit,
                        options.RateUnit,
                        allTags,
                        options.ResetOnReporting,
                        options.ReportSetItems);
                    return Tuple.Create((IMeter)meter, valueSource);
                });
        }

        /// <inheritdoc />
        public IMeter Meter<T>(MeterOptions options, MetricTags tags, Func<T> builder)
            where T : IMeterMetric
        {
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _meters.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Meter {Name} - {@Options} {MesurementUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), allTags.ToDictionary());

                    var meter = builder();
                    var valueSource = new MeterValueSource(
                        metricName,
                        meter,
                        options.MeasurementUnit,
                        options.RateUnit,
                        allTags,
                        options.ResetOnReporting,
                        options.ReportSetItems);
                    return Tuple.Create((IMeter)meter, valueSource);
                });
        }

        public ITimer Timer<T>(TimerOptions options, Func<T> builder)
            where T : ITimerMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _timers.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Timer {Name} - {@Options} {MesurementUnit} {DurationUnit} {RateUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), options.DurationUnit, options.RateUnit, allTags.ToDictionary());

                    var timer = builder();
                    var valueSource = new TimerValueSource(
                    metricName,
                        timer,
                        options.MeasurementUnit,
                        options.RateUnit,
                        options.DurationUnit,
                        allTags,
                        options.ResetOnReporting);
                    return Tuple.Create((ITimer)timer, valueSource);
                });
        }

        /// <inheritdoc />
        public ITimer Timer<T>(TimerOptions options, MetricTags tags, Func<T> builder)
            where T : ITimerMetric
        {
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _timers.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Timer {Name} - {@Options} {MesurementUnit} {DurationUnit} {RateUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), options.DurationUnit, options.RateUnit, allTags.ToDictionary());

                    var timer = builder();
                    var valueSource = new TimerValueSource(
                        metricName,
                        timer,
                        options.MeasurementUnit,
                        options.RateUnit,
                        options.DurationUnit,
                        allTags,
                        options.ResetOnReporting);
                    return Tuple.Create((ITimer)timer, valueSource);
                });
        }

        public ITimer BucketTimer<T>(BucketTimerOptions options, Func<T> builder)
            where T : IBucketTimerMetric
        {
            var allTags = AllTags(options.Tags);
            var metricName = allTags.AsMetricName(options.Name);

            return _bucketTimers.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Timer {Name} - {@Options} {MesurementUnit} {DurationUnit} {RateUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), options.DurationUnit, options.RateUnit, allTags.ToDictionary());

                    var timer = builder();
                    var valueSource = new BucketTimerValueSource(
                    metricName,
                        timer,
                        options.MeasurementUnit,
                        options.RateUnit,
                        options.DurationUnit,
                        allTags);
                    return Tuple.Create((ITimer)timer, valueSource);
                });
        }

        /// <inheritdoc />
        public ITimer BucketTimer<T>(BucketTimerOptions options, MetricTags tags, Func<T> builder)
            where T : IBucketTimerMetric
        {
            var allTags = AllTags(MetricTags.Concat(options.Tags, tags));
            var metricName = allTags.AsMetricName(options.Name);

            return _bucketTimers.GetOrAdd(
                metricName,
                () =>
                {
                    Logger.Trace("Adding Timer {Name} - {@Options} {MesurementUnit} {DurationUnit} {RateUnit} {@Tags}", metricName, options, options.MeasurementUnit.ToString(), options.DurationUnit, options.RateUnit, allTags.ToDictionary());

                    var timer = builder();
                    var valueSource = new BucketTimerValueSource(
                        metricName,
                        timer,
                        options.MeasurementUnit,
                        options.RateUnit,
                        options.DurationUnit,
                        allTags);
                    return Tuple.Create((ITimer)timer, valueSource);
                });
        }

        private MetricTags AllTags(MetricTags metricTags)
        {
            return MetricTags.Concat(
                MetricTags.Concat(metricTags, _globalTags),
                _contextualTags.ComputeTagValues());
        }

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
