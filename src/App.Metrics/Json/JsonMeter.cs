using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Json
{
    public class JsonMeter : JsonMetric
    {
        private SetItem[] items = new SetItem[0];

        public long Count { get; set; }

        public double FifteenMinuteRate { get; set; }

        public double FiveMinuteRate { get; set; }

        public SetItem[] Items
        {
            get { return this.items; }
            set { this.items = value ?? new SetItem[0]; }
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
                Tags = meter.Tags
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
            yield return new JsonProperty("MeanRate", this.MeanRate);
            yield return new JsonProperty("OneMinuteRate", this.OneMinuteRate);
            yield return new JsonProperty("FiveMinuteRate", this.FiveMinuteRate);
            yield return new JsonProperty("FifteenMinuteRate", this.FifteenMinuteRate);
            yield return new JsonProperty("Unit", this.Unit);
            yield return new JsonProperty("RateUnit", this.RateUnit);

            if (this.Items.Length > 0)
            {
                yield return new JsonProperty("Items", this.Items.Select(i => new JsonObject(ToJsonProperties(i))));
            }

            if (this.Tags.Length > 0)
            {
                yield return new JsonProperty("Tags", this.Tags);
            }
        }

        public MeterValueSource ToValueSource()
        {
            var rateUnit = TimeUnitExtensions.FromUnit(this.RateUnit);

            var items = this.Items.Select(i =>
                    new MeterValue.SetItem(i.Item, i.Percent,
                        new MeterValue(i.Count, i.MeanRate, i.OneMinuteRate, i.FiveMinuteRate, i.FifteenMinuteRate, rateUnit)))
                .ToArray();

            var meterValue = new MeterValue(this.Count, this.MeanRate, this.OneMinuteRate, this.FiveMinuteRate, this.FifteenMinuteRate, rateUnit,
                items);
            return new MeterValueSource(this.Name, ConstantValue.Provider(meterValue), this.Unit, rateUnit, this.Tags);
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