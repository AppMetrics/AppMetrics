using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public static class MeterConverterExtensions
    {
        public static MeterValueSource FromJson(this JsonMeter source)
        {
            var rateUnit = source.RateUnit.FromUnit();
            var items = source.Items.Select(i =>
                new MeterValue.SetItem(i.Item, i.Percent,
                    new MeterValue(i.Count, i.MeanRate, i.OneMinuteRate, i.FiveMinuteRate, i.FifteenMinuteRate, rateUnit))).ToArray();

            var meterValue = new MeterValue(source.Count, source.MeanRate,
                source.OneMinuteRate, source.FiveMinuteRate,
                source.FifteenMinuteRate, rateUnit, items);

            return new MeterValueSource(source.Name, ConstantValue.Provider(meterValue), source.Unit, rateUnit, source.Tags);
        }

        public static IEnumerable<JsonMeter> ToJson(this IEnumerable<MeterValueSource> source)
        {
            return source.Select(x => x.ToJson());
        }

        public static JsonMeter ToJson(this MeterValueSource source)
        {
            var items = source.Value.Items.Select(i =>
                new JsonMeter.SetItem
                {
                    Count = i.Value.Count,
                    OneMinuteRate = i.Value.OneMinuteRate,
                    FiveMinuteRate = i.Value.FiveMinuteRate,
                    FifteenMinuteRate = i.Value.FifteenMinuteRate,
                    MeanRate = i.Value.MeanRate,
                    Item = i.Item,
                    Percent = i.Percent
                }).ToArray();

            return new JsonMeter
            {
                RateUnit = source.Value.RateUnit.Unit(),
                Items = items,
                Count = source.Value.Count,
                Name = source.Name,
                Unit = source.Unit.Name,
                OneMinuteRate = source.Value.OneMinuteRate,
                FiveMinuteRate = source.Value.FiveMinuteRate,
                FifteenMinuteRate = source.Value.FifteenMinuteRate,
                MeanRate = source.Value.MeanRate,
                Tags = source.Tags.ToDictionary()
            };
        }

        public static IEnumerable<MeterValueSource> FromJson(this IEnumerable<JsonMeter> source)
        {
            return source.Select(x => x.FromJson());
        }
    }
}