// <copyright file="CounterConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Counter;

namespace App.Metrics.Formatters.Json.Converters
{
    public class CounterConverter : JsonConverter<CounterValueSource>
    {
        public override bool CanConvert(Type objectType) { return typeof(CounterValueSource) == objectType; }
        
        public override CounterValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<CounterMetric>(reader.GetString());
            return source.FromSerializableMetric();
        }

        public override void Write(Utf8JsonWriter writer, CounterValueSource value, JsonSerializerOptions options)
        {
            var target = value.ToSerializableMetric();

            JsonSerializer.Serialize(writer, target);
        }
    }
}