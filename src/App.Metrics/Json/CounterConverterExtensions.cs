using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public static class CounterConverterExtensions
    {
        public static CounterValueSource FromJson(this JsonCounter source)
        {
            var items = source.Items.Select(i => new CounterValue.SetItem(i.Item, i.Count, i.Percent)).ToArray();
            var counterValue = new CounterValue(source.Count, items);
            return new CounterValueSource(source.Name, ConstantValue.Provider(counterValue), source.Unit, source.Tags);
        }

        public static IEnumerable<CounterValueSource> FromJson(this IEnumerable<JsonCounter> source)
        {
            return source.Select(x => x.FromJson());
        }

        public static IEnumerable<JsonCounter> ToJson(this IEnumerable<CounterValueSource> source)
        {
            return source.Select(x => x.ToJson());
        }

        public static JsonCounter ToJson(this CounterValueSource source)
        {
            return new JsonCounter
            {
                Name = source.Name,
                Count = source.Value.Count,
                Unit = source.Unit.Name,
                Items = source.Value.Items.Select(item => new JsonCounter.SetItem
                {
                    Count = item.Count,
                    Percent = item.Percent,
                    Item = item.Item
                }).ToArray(),
                Tags = source.Tags.ToDictionary()
            };
        }
    }
}