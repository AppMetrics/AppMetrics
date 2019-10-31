// <copyright file="GaugeConverter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Metrics.Gauge;

namespace App.Metrics.Formatters.Json.Converters
{
    public class GaugeConverter : JsonConverter<GaugeValueSource>
    {
        public override bool CanConvert(Type objectType) { return typeof(GaugeValueSource) == objectType; }
        
        public override GaugeValueSource Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = JsonSerializer.Deserialize<GaugeMetric>(reader.GetString());

            return source.FromSerializableMetric();
        }

        public override void Write(Utf8JsonWriter writer, GaugeValueSource value, JsonSerializerOptions options)
        {
            var target = value.ToSerializableMetric();

            JsonSerializer.Serialize(writer, target);
        }
    }
}