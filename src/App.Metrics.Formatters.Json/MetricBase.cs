// <copyright file="MetricBase.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public abstract class MetricBase
    {
        [JsonProperty(Order = -3)]
        public string Name { get; set; }

        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        [JsonProperty(Order = -2)]
        public string Unit { get; set; }
    }
}