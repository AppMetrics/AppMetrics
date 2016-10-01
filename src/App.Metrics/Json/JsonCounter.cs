using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public class JsonCounter : JsonMetric
    {
        private SetItem[] items = new SetItem[0];

        public long Count { get; set; }

        public SetItem[] Items
        {
            get { return this.items; }
            set { this.items = value ?? new SetItem[0]; }
        }

        public static JsonCounter FromCounter(MetricValueSource<CounterValue> counter)
        {
            return new JsonCounter
            {
                Name = counter.Name,
                Count = counter.Value.Count,
                Unit = counter.Unit.Name,
                Items = counter.Value.Items.Select(i => new SetItem { Item = i.Item, Count = i.Count, Percent = i.Percent }).ToArray(),
                Tags = counter.Tags
            };
        }

        public JsonObject ToJsonObject()
        {
            return new JsonObject(ToJsonProperties());
        }

        public IEnumerable<JsonProperty> ToJsonProperties()
        {
            yield return new JsonProperty("Name", this.Name);
            yield return new JsonProperty("Count", this.Count);
            yield return new JsonProperty("Unit", this.Unit);

            if (this.Items.Length > 0)
            {
                yield return new JsonProperty("Items", this.Items.Select(i => new JsonObject(ToJsonProperties(i))));
            }

            if (this.Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", this.Tags);
            }
        }

        public CounterValueSource ToValueSource()
        {
            var items = this.items.Select(i => new CounterValue.SetItem(i.Item, i.Count, i.Percent))
                .ToArray();

            var counterValue = new CounterValue(this.Count, items);

            return new CounterValueSource(this.Name, ConstantValue.Provider(counterValue), this.Unit, this.Tags);
        }

        private static IEnumerable<JsonProperty> ToJsonProperties(SetItem item)
        {
            yield return new JsonProperty("Item", item.Item);
            yield return new JsonProperty("Count", item.Count);
            yield return new JsonProperty("Percent", item.Percent);
        }

        public class SetItem
        {
            public long Count { get; set; }

            public string Item { get; set; }

            public double Percent { get; set; }
        }
    }
}