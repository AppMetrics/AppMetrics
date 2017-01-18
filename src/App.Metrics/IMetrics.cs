// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
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
        IProvideMetrics Provider { get; }

        IBuildMetrics Build { get; }

        IClock Clock { get; }

        IProvideMetricValues Snapshot { get; }

        IFilterMetrics GlobalFilter { get; }

        GlobalMetricTags GlobalTags { get; }

        IProvideHealth Health { get; }

        IManageMetrics Manage { get; }

        IMeasureMetrics Measure { get; }
    }
}