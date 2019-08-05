// <copyright file="DefaultMetricRegistryManager.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Registry;
using App.Metrics.Timer;

namespace App.Metrics.Internal
{
    public sealed class DefaultMetricRegistryManager : IMetricRegistryManager
    {
        private readonly Func<IEnumerable<ApdexValueSource>> _apdexScores;
        private readonly Func<IEnumerable<CounterValueSource>> _counters;
        private readonly Func<IEnumerable<GaugeValueSource>> _gauges;
        private readonly Func<IEnumerable<HistogramValueSource>> _histograms;
        private readonly Func<IEnumerable<BucketHistogramValueSource>> _bucketHistograms;
        private readonly Func<IEnumerable<MeterValueSource>> _meters;
        private readonly Func<IEnumerable<TimerValueSource>> _timers;
        private readonly Func<IEnumerable<BucketTimerValueSource>> _bucketTimers;

        public DefaultMetricRegistryManager(
            Func<IEnumerable<GaugeValueSource>> gauges,
            Func<IEnumerable<CounterValueSource>> counters,
            Func<IEnumerable<MeterValueSource>> meters,
            Func<IEnumerable<HistogramValueSource>> histograms,
            Func<IEnumerable<BucketHistogramValueSource>> bucketHistograms,
            Func<IEnumerable<TimerValueSource>> timers,
            Func<IEnumerable<BucketTimerValueSource>> bucketTimers,
            Func<IEnumerable<ApdexValueSource>> apdexScores)
        {
            _gauges = gauges;
            _counters = counters;
            _meters = meters;
            _histograms = histograms;
            _timers = timers;
            _bucketTimers = bucketTimers;
            _apdexScores = apdexScores;
            _bucketHistograms = bucketHistograms;
        }

        /// <inheritdoc />
        public IEnumerable<ApdexValueSource> ApdexScores => _apdexScores();

        /// <inheritdoc />
        public IEnumerable<CounterValueSource> Counters => _counters();

        /// <inheritdoc />
        public IEnumerable<GaugeValueSource> Gauges => _gauges();

        /// <inheritdoc />
        public IEnumerable<HistogramValueSource> Histograms => _histograms();

        /// <inheritdoc />
        public IEnumerable<BucketHistogramValueSource> BucketHistograms => _bucketHistograms();

        /// <inheritdoc />
        public IEnumerable<MeterValueSource> Meters => _meters();

        /// <inheritdoc />
        public IEnumerable<TimerValueSource> Timers => _timers();

        /// <inheritdoc />
        public IEnumerable<BucketTimerValueSource> BucketTimers => _bucketTimers();
    }
}