// <copyright file="HistogramConverter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class HistogramConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return typeof(HistogramValueSource) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<HistogramMetric>(reader);

            return source.FromSerializableMetric();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (HistogramValueSource)value;

            var target = source.ToSerializableMetric();

            serializer.Serialize(writer, target);
        }
    }
}