// <copyright file="ApdexConverter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class ApdexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return typeof(ApdexValueSource) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ApdexMetric>(reader);
            return source.FromSerializableMetric();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (ApdexValueSource)value;

            var target = source.ToSerializableMetric();

            serializer.Serialize(writer, target);
        }
    }
}