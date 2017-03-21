// <copyright file="CounterConverter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Counter;
using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json.Converters
{
    public class CounterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return typeof(CounterValueSource) == objectType; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<CounterMetric>(reader);
            return source.FromSerializableMetric();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (CounterValueSource)value;

            var target = source.ToSerializableMetric();

            serializer.Serialize(writer, target);
        }
    }
}