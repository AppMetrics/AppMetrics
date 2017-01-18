// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultMetricsManagerFactory : IMetricsManagerFactory
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetricsManagerFactory" /> class.
        /// </summary>
        /// <param name="registry">The metrics registry.</param>
        /// <param name="buideFactory">The buide factory.</param>
        /// <param name="clock">The clock.</param>
        public DefaultMetricsManagerFactory(IMetricsRegistry registry, IMetricsBuilderFactory buideFactory, IClock clock)
        {
            ApdexManager = new DefaultApdexManager(buideFactory.Apdex, registry, clock);
            CounterManager = new DefaultCounterManager(buideFactory.Counter, registry);
            GaugeManager = new DefaultGaugeManager(buideFactory.Gauge, registry);
            HistogramManager = new DefaultHistogramManager(buideFactory.Histogram, registry);
            MeterManager = new DefaultMeterManager(buideFactory.Meter, registry, clock);
            TimerManager = new DefaultTimerManager(buideFactory.Timer, registry, clock);
        }

        public IMeasureApdexMetrics ApdexManager { get; }

        public IMeasureCounterMetrics CounterManager { get; }

        public IMeasureGaugeMetrics GaugeManager { get; }

        public IMeasureHistogramMetrics HistogramManager { get; }

        public IMeasureMeterMetrics MeterManager { get; }

        public IMeasureTimerMetrics TimerManager { get; }
    }
}