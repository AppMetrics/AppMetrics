using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
{
    public sealed class MetricGroup
    {
        public IEnumerable<Counter> Counters { get; set; } = Enumerable.Empty<Counter>();

        public IEnumerable<Gauge> Gauges { get; set; } = Enumerable.Empty<Gauge>();

        public string GroupName { get; set; }

        public IEnumerable<Histogram> Histograms { get; set; } = Enumerable.Empty<Histogram>();

        public IEnumerable<Meter> Meters { get; set; } = Enumerable.Empty<Meter>();

        public IEnumerable<Timer> Timers { get; set; } = Enumerable.Empty<Timer>();
    }
}