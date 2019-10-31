// <copyright file="TimerConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Timer;

namespace App.Metrics.Formatters.Json.Converters
{
    public class TimerConverter : JsonConverter<TimerValueSource>
    {
        public override bool CanConvert(Type objectType) { return typeof(TimerValueSource) == objectType; }
        
        public override TimerValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<TimerMetric>(reader.GetString());

            return source.FromSerializableMetric();
        }

        public override void Write(Utf8JsonWriter writer, TimerValueSource value, JsonSerializerOptions options)
        {
            var target = value.ToSerializableMetric();

            JsonSerializer.Serialize(writer, target);
        }
    }
}