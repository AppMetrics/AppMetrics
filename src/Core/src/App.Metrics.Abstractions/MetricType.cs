// <copyright file="MetricType.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    /// <summary>
    ///     Available metric types
    /// </summary>
    public enum MetricType
    {
        Gauge,

        Counter,

        Meter,

        Histogram,

        BucketHistogram,

        Timer,

        BucketTimer,

        Apdex
    }
}