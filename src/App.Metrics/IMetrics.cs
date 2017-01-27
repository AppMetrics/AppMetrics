// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.Filtering;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Interfaces;
using App.Metrics.Health.Abstractions;

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
        IBuildMetrics Build { get; }

        IClock Clock { get; }

        IFilterMetrics GlobalFilter { get; }

        IProvideHealth Health { get; }

        IManageMetrics Manage { get; }

        IMeasureMetrics Measure { get; }

        IProvideMetrics Provider { get; }

        IProvideMetricValues Snapshot { get; }
    }
}