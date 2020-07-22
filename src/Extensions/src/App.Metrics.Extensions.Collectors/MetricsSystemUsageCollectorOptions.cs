// <copyright file="MetricsSystemUsageCollectorOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Extensions.Collectors
{
    public class MetricsSystemUsageCollectorOptions
    {
        public int CollectIntervalMilliseconds { get; set; } = 5000;
    }
}