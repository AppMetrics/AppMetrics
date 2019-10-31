// <copyright file="MeterConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Meter;

namespace App.Metrics.Formatters.Json.Converters
{
    public class MeterConverter : JsonConverter<MeterValueSource>
    {
        public override bool CanConvert(Type objectType) { return typeof(MeterValueSource) == objectType; }
        
        public override MeterValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<MeterMetric>(reader.GetString());

            return source.FromSerializableMetric();
        }

        public override void Write(Utf8JsonWriter writer, MeterValueSource value, JsonSerializerOptions options)
        {
            var target = value.ToSerializableMetric();

            JsonSerializer.Serialize(writer, target);
        }
    }
}