using System;
using System.Collections.Generic;

namespace App.Metrics.Json
{
    public sealed class JsonMetricsData
    {
        public string ContextName { get; set; }

        public IDictionary<string, string> Environment { get; set; }

        public IEnumerable<JsonMetricsGroup> Groups { get; set; } = new JsonMetricsGroup[0];

        public DateTime Timestamp { get; set; }

        public string Version { get; set; }
    }
}