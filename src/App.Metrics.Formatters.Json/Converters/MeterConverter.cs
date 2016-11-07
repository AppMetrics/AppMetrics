using System;
using App.Metrics.Data;
using App.Metrics.Extensions;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class MeterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(MeterValueSource) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<Meter>(reader);

            return source.ToMetricValueSource();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (MeterValueSource)value;

            var target = source.ToMetric();

            serializer.Serialize(writer, target);
        }
    }
}