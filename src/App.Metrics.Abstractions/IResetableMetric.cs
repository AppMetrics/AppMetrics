// <copyright file="IResetableMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
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