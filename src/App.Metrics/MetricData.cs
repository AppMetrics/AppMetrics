using System;
using System.Collections.Generic;

namespace App.Metrics
{
    public sealed class MetricData
    {
        public string ContextName { get; set; }

        public IDictionary<string, string> Environment { get; set; }

        public IEnumerable<MetricGroup> Groups { get; set; } = new MetricGroup[0];

        public DateTime Timestamp { get; set; }

        public string Version { get; set; }
    }
}