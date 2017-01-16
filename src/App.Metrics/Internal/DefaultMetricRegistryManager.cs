// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using App.Metrics.Data;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal
{
    internal sealed class DefaultMetricRegistryManager : IMetricRegistryManager
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

        public IEnumerable<ApdexValueSource> ApdexScores => _apdexScores();

        public IEnumerable<CounterValueSource> Counters => _counters();

        public IEnumerable<GaugeValueSource> Gauges => _gauges();

        public IEnumerable<HistogramValueSource> Histograms => _histograms();

        public IEnumerable<MeterValueSource> Meters => _meters();

        public IEnumerable<TimerValueSource> Timers => _timers();
    }
}