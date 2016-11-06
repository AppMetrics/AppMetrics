using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Json
{
    public sealed class JsonMeter : JsonMetric
    {
        public long Count { get; set; }

        public double FifteenMinuteRate { get; set; }

        public double FiveMinuteRate { get; set; }

        public IEnumerable<SetItem> Items { get; set; } = Enumerable.Empty<SetItem>();

        public double MeanRate { get; set; }

        public double OneMinuteRate { get; set; }

        public string RateUnit { get; set; }

        public class SetItem
        {
            public long Count { get; set; }

            public double FifteenMinuteRate { get; set; }

            public double FiveMinuteRate { get; set; }

            public string Item { get; set; }

            public double MeanRate { get; set; }

            public double OneMinuteRate { get; set; }

            public double Percent { get; set; }
        }
    }
}