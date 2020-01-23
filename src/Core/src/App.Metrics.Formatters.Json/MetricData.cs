// <copyright file="MetricData.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.Formatters.Json
{
    public sealed class MetricData
    {
        public IEnumerable<MetricsContext> Contexts { get; set; } = new MetricsContext[0];

        public DateTime Timestamp { get; set; }
    }
}