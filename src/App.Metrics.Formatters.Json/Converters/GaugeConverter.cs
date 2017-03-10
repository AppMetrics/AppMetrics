// <copyright file="GaugeConverter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Gauge;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class GaugeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return typeof(GaugeValueSource) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<GaugeMetric>(reader);

            return source.FromSerializableMetric();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (GaugeValueSource)value;

            var target = source.ToSerializableMetric();

            serializer.Serialize(writer, target);
        }
    }
}