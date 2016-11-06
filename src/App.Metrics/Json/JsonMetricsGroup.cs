using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Json
{
    public sealed class JsonMetricsGroup
    {
        public IEnumerable<JsonCounter> Counters { get; set; } = Enumerable.Empty<JsonCounter>();

        public IEnumerable<JsonGauge> Gauges { get; set; } = Enumerable.Empty<JsonGauge>();

        public string GroupName { get; set; }

        public IEnumerable<JsonHistogram> Histograms { get; set; } = Enumerable.Empty<JsonHistogram>();

        public IEnumerable<JsonMeter> Meters { get; set; } = Enumerable.Empty<JsonMeter>();

        public IEnumerable<JsonTimer> Timers { get; set; } = Enumerable.Empty<JsonTimer>();
    }
}