// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Json
{
    public class JsonMetric
    {
        private Dictionary<string, string> _tags = MetricTags.None;

        public string Name { get; set; }

        public Dictionary<string, string>  Tags
        {
            get { return _tags; }
            set { _tags = value ?? new Dictionary<string, string>(); }
        }

        public string Unit { get; set; }
    }
}