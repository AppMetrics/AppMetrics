// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Managers
{
    internal interface IMetricsManagerFactory
    {
        IMeasureApdexMetrics ApdexManager { get; }

        IMeasureCounterMetrics CounterManager { get; }

        IMeasureGaugeMetrics GaugeManager { get; }

        IMeasureHistogramMetrics HistogramManager { get; }

        IMeasureMeterMetrics MeterManager { get; }

        IMeasureTimerMetrics TimerManager { get; }
    }
}