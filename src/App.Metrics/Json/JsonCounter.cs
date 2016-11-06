using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Json
{
    public sealed class JsonCounter : JsonMetric
    {
        public long Count { get; set; }

        public IEnumerable<SetItem> Items { get; set; } = Enumerable.Empty<SetItem>();

        public class SetItem
        {
            public long Count { get; set; }

            public string Item { get; set; }

            public double Percent { get; set; }
        }
    }
}