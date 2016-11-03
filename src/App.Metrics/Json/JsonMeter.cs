// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Json
{
    public sealed class JsonMeter : JsonMetric
    {
        private SetItem[] items = new SetItem[0];

        public long Count { get; set; }

        public double FifteenMinuteRate { get; set; }

        public double FiveMinuteRate { get; set; }

        public SetItem[] Items
        {
            get { return items; }
            set { items = value ?? new SetItem[0]; }
        }

        public double MeanRate { get; set; }

        public double OneMinuteRate { get; set; }

        public string RateUnit { get; set; }

        public static JsonMeter FromMeter(MeterValueSource meter)
        {
            return new JsonMeter
            {
                Name = meter.Name,
                Count = meter.Value.Count,
                MeanRate = meter.Value.MeanRate,
                OneMinuteRate = meter.Value.OneMinuteRate,
                FiveMinuteRate = meter.Value.FiveMinuteRate,
                FifteenMinuteRate = meter.Value.FifteenMinuteRate,
                Unit = meter.Unit.Name,
                RateUnit = meter.RateUnit.Unit(),
                Items = meter.Value.Items.Select(i => new SetItem
                {
                    Item = i.Item,
                    Count = i.Value.Count,
                    MeanRate = i.Value.MeanRate,
                    OneMinuteRate = i.Value.OneMinuteRate,
                    FiveMinuteRate = i.Value.FiveMinuteRate,
                    FifteenMinuteRate = i.Value.FifteenMinuteRate,
                    Percent = i.Percent
                }).ToArray(),
                Tags = meter.Tags.ToDictionary()
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
            yield return new JsonProperty("MeanRate", MeanRate);
            yield return new JsonProperty("OneMinuteRate", OneMinuteRate);
            yield return new JsonProperty("FiveMinuteRate", FiveMinuteRate);
            yield return new JsonProperty("FifteenMinuteRate", FifteenMinuteRate);
            yield return new JsonProperty("Unit", Unit);
            yield return new JsonProperty("RateUnit", RateUnit);

            if (Items.Length > 0)
            {
                yield return new JsonProperty("Items", Items.Select(i => new JsonObject(ToJsonProperties(i))));
            }

            //TODO: AH - rather than custom serializer use json.net
            //if (Tags.Count > 0)
            //{
            //    yield return new JsonProperty("Tags", Tags);
            //}
        }

        public MeterValueSource ToValueSource()
        {
            var rateUnit = TimeUnitExtensions.FromUnit(RateUnit);

            var items = Items.Select(i =>
                    new MeterValue.SetItem(i.Item, i.Percent,
                        new MeterValue(i.Count, i.MeanRate, i.OneMinuteRate, i.FiveMinuteRate, i.FifteenMinuteRate, rateUnit)))
                .ToArray();

            var meterValue = new MeterValue(Count, MeanRate, OneMinuteRate, FiveMinuteRate, FifteenMinuteRate, rateUnit,
                items);
            return new MeterValueSource(Name, ConstantValue.Provider(meterValue), Unit, rateUnit, Tags);
        }

        private static IEnumerable<JsonProperty> ToJsonProperties(SetItem item)
        {
            yield return new JsonProperty("Item", item.Item);
            yield return new JsonProperty("Count", item.Count);
            yield return new JsonProperty("MeanRate", item.MeanRate);
            yield return new JsonProperty("OneMinuteRate", item.OneMinuteRate);
            yield return new JsonProperty("FiveMinuteRate", item.FiveMinuteRate);
            yield return new JsonProperty("FifteenMinuteRate", item.FifteenMinuteRate);
            yield return new JsonProperty("Percent", item.Percent);
        }

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