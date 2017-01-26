// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Core.Abstractions
{
    /// <summary>
    ///     Indicates a metric's ability to be reset. Reseting a metric clear all currently collected data.
    /// </summary>
    public interface IResetableMetric
    {
        /// <summary>
        ///     Clear all currently collected data for this metric.
        /// </summary>
        void Reset();
    }
}