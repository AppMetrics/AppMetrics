// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public sealed class JsonCounter : JsonMetric
    {
        private SetItem[] _items = new SetItem[0];

        public long Count { get; set; }

        public SetItem[] Items
        {
            get { return _items; }
            set { _items = value ?? new SetItem[0]; }
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
            yield return new JsonProperty("Name", Name);
            yield return new JsonProperty("Count", Count);
            yield return new JsonProperty("Unit", Unit);

            if (Items.Length > 0)
            {
                yield return new JsonProperty("Items", Items.Select(i => new JsonObject(ToJsonProperties(i))));
            }

            if (Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", Tags);
            }
        }

        public CounterValueSource ToValueSource()
        {
            var items = _items.Select(i => new CounterValue.SetItem(i.Item, i.Count, i.Percent))
                .ToArray();

            var counterValue = new CounterValue(Count, items);

            return new CounterValueSource(Name, ConstantValue.Provider(counterValue), Unit, Tags);
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