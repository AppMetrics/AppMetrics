// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public abstract class MetricBase
    {
        [JsonProperty(Order = -3)]
        public string Name { get; set; }

        [JsonProperty(Order = -4)]
        public string Group { get; set; }

        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        [JsonProperty(Order = -2)]
        public string Unit { get; set; }
    }
}