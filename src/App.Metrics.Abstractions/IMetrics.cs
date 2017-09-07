// <copyright file="IMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Filters;

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

        IFilterMetrics Filter { get; }

        IManageMetrics Manage { get; }

        IMeasureMetrics Measure { get; }

        IProvideMetrics Provider { get; }

        IProvideMetricValues Snapshot { get; }
    }
}