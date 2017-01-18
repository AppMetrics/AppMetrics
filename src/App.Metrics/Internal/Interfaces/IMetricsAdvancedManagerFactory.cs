// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Interfaces
{
    public interface IMetricsAdvancedManagerFactory
    {
        IMeasureApdexMetricsAdvanced Apdex { get; }

        IMeasureCounterMetricsAdvanced Counter { get; }

        IMeasureGaugeMetricsAdvanced Gauge { get; }

        IMeasureHistogramMetricsAdvanced Histogram { get; }

        IMeasureMeterMetricsAdvanced Meter { get; }

        IMeasureTimerMetricsAdvanced Timer { get; }
    }
}