// <copyright file="DefaultMetricRegistryManager.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Apdex;
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
        private readonly Func<IEnumerable<MeterValueSource>> _meters;
        private readonly Func<IEnumerable<TimerValueSource>> _timers;

        public DefaultMetricRegistryManager(
            Func<IEnumerable<GaugeValueSource>> gauges,
            Func<IEnumerable<CounterValueSource>> counters,
            Func<IEnumerable<MeterValueSource>> meters,
            Func<IEnumerable<HistogramValueSource>> histograms,
            Func<IEnumerable<TimerValueSource>> timers,
            Func<IEnumerable<ApdexValueSource>> apdexScores)
        {
            _gauges = gauges;
            _counters = counters;
            _meters = meters;
            _histograms = histograms;
            _timers = timers;
            _apdexScores = apdexScores;
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
        public IEnumerable<MeterValueSource> Meters => _meters();

        /// <inheritdoc />
        public IEnumerable<TimerValueSource> Timers => _timers();
    }
}