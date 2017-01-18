// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     Gets the record application metrics.
    /// </summary>
    /// <remarks>
    ///     This is the entry point to the application's metrics registry
    /// </remarks>
    public interface IMetrics
    {
        IMetricsAdvancedManagerFactory AdvancedMetrics { get; }

        IMeasureApdexMetrics Apdex { get; }

        // TODO: ! Add docs

        IMetricsBuilderFactory Build { get; }

        IClock Clock { get; }

        IMeasureCounterMetrics Counter { get; }

        IMetricsDataProvider Data { get; }

        IMeasureGaugeMetrics Gauge { get; }

        IMetricsFilter GlobalFilter { get; }

        GlobalMetricTags GlobalTags { get; }

        IHealthStatusProvider Health { get; }

        IMeasureHistogramMetrics Histogram { get; }

        IManageMetrics Manage { get; }

        IMeasureMeterMetrics Meter { get; }

        IMeasureTimerMetrics Timer { get; }
    }
}