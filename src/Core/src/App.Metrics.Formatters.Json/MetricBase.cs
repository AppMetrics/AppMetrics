// <copyright file="MetricBase.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Formatters.Json
{
    public abstract class MetricBase
    {
        public string Name { get; set; }

        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        public string Unit { get; set; }
    }
}