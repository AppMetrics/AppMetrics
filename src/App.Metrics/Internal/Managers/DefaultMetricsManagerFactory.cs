// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultMetricsManagerFactory : IMetricsManagerFactory
    {
        public DefaultMetricsManagerFactory(IMetricsRegistry registry, IAdvancedMetrics advancedManager)
        {
            ApdexManager = new DefaultApdexManager(advancedManager, registry);
            CounterManager = new DefaultCounterManager(advancedManager, registry);
            GaugeManager = new DefaultGaugeManager(advancedManager, registry);
            HistogramManager = new DefaultHistogramManager(advancedManager, registry);
            MeterManager = new DefaultMeterManager(advancedManager, registry);
            TimerManager = new DefaultTimerManager(advancedManager, registry);
        }

        public IMeasureApdexMetrics ApdexManager { get; }

        public IMeasureCounterMetrics CounterManager { get; }

        public IMeasureGaugeMetrics GaugeManager { get; }

        public IMeasureHistogramMetrics HistogramManager { get; }

        public IMeasureMeterMetrics MeterManager { get; }

        public IMeasureTimerMetrics TimerManager { get; }
    }
}