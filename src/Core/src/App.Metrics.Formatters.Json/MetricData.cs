// <copyright file="MetricData.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public sealed class MetricData
    {
        [JsonProperty(Order = 2)]
        public IEnumerable<MetricsContext> Contexts { get; set; } = new MetricsContext[0];

        [JsonProperty(Order = 1)]
        public DateTime Timestamp { get; set; }
    }
}